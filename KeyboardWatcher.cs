using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SendMedia
{
    public class KeyboardWatcher
    {
        public delegate void ActiveWindowHandler();
        public event ActiveWindowHandler ActivatedWindow;
        public event ActiveWindowHandler DeactivatedWindow;

        public delegate void ActiveKeywordHandler(string keyword, string word);
        public event ActiveKeywordHandler ActivatedKeyword;
        public event ActiveKeywordHandler DeactivatedKeyword;

        private List<string> _windowTitles;
        private List<string> _ignoreWindowTitles;

        private KeyboardHook _kh = new KeyboardHook(true);
        private ActiveWindowHook _awk = new ActiveWindowHook();

        private DateTime _lastPressedTime;

        private bool _isActivatedWindow = false;

        enum KeywordEnum
        {
            None,
            Started,
            Ended
        }

        private KeywordEnum _keywordState = KeywordEnum.None;

        public KeyboardWatcher(List<string> windowTitles, List<string> ignoreWindowTitles)
        {
            _windowTitles = windowTitles;
            _ignoreWindowTitles = ignoreWindowTitles;

            _awk.Activated += Awk_ActiveWindow;
        }

        const string _keyword = "/GIF ";
        private string _sentence = string.Empty;

        private string GetKeyStr(Keys key, bool shift)
        {
            if (key == Keys.D2 && shift)
            {
                return "@";
            }
            else if (key == Keys.Oem1 && shift)
            {
                return ":";
            }
            else if (key == Keys.Space)
            {
                return " ";
            }
            else if (key == Keys.Divide)
            {
                return "/";
            }
            else if (key == Keys.LShiftKey || key == Keys.RShiftKey)
            {
                return string.Empty;
            }
            else
            {
                return key.ToString();
            }
        }

        private object _locker = new object();

        private const int _delayInSeconds = 2;

        private void Kh_KeyDown(Keys key, bool shift, bool ctrl, bool alt)
        {
            //Console.WriteLine("The Key: " + key);

            Debug.WriteLine($"Key: {key.ToString()} Shift: { shift.ToString()} \n");

            string keyStr = GetKeyStr(key, shift);

            if (string.IsNullOrEmpty(keyStr))
                return;

            if (key != Keys.Back)
            {
                _sentence += keyStr.ToString();

                if (_sentence.Length > 1024)
                {
                    _sentence = _sentence.Substring(1);
                }
            }
            else
            {
                if (_sentence.Length > 0)
                {
                    _sentence = _sentence.Substring(0, _sentence.Length - 1);
                }
            }

            Debug.WriteLine(_sentence);

            if (_keywordState == KeywordEnum.None)
            {
                if (_sentence.Contains(_keyword))
                {
                    _keywordState = KeywordEnum.Started;
                }
            }
            else if (_keywordState == KeywordEnum.Started)
            {
                if (!string.IsNullOrEmpty(_sentence))
                {
                    ThreadPool.QueueUserWorkItem(
                        x =>
                        {
                            Thread.Sleep(TimeSpan.FromSeconds(_delayInSeconds));

                            lock (_locker)
                            {
                                if (DateTime.Now.Subtract(_lastPressedTime).TotalSeconds >= _delayInSeconds)
                                {
                                    _keywordState = KeywordEnum.Ended;

                                    var word = _sentence.Substring(_sentence.LastIndexOf(_keyword) + _keyword.Length).Trim();

                                    if (!string.IsNullOrEmpty(word))
                                    {
                                        ActivatedKeyword(_keyword, word);

                                        _lastPressedTime = DateTime.Now;
                                    }
                                }
                            }
                        });
                }
            }
            else if (_keywordState == KeywordEnum.Ended)
            {
                _keywordState = KeywordEnum.None;

                DeactivatedKeyword(_keyword, null);

                _sentence = string.Empty;
            }

            _lastPressedTime = DateTime.Now;
        }

        public bool IsActivatedWindow
        {
            set
            {
                if (value)
                {
                    if (!_isActivatedWindow)
                    {
                        _isActivatedWindow = true;

                        _kh.KeyDown += Kh_KeyDown;
                    }
                }
                else
                {
                    if (_isActivatedWindow)
                    {
                        _isActivatedWindow = false;

                        _kh.KeyDown -= Kh_KeyDown;
                    }
                }
            }
            get
            {
                return _isActivatedWindow;
            }
        }

        private void Awk_ActiveWindow(string title)
        {
            if (!_ignoreWindowTitles.Any(x => title.Contains(x)))
            {
                if (_windowTitles.Any(x => title.Contains(x)))
                {
                    if (ActivatedWindow != null)
                    {
                        ActivatedWindow();
                    }

                   IsActivatedWindow = true;
                }
                else if (_isActivatedWindow)
                {
                    IsActivatedWindow = false;

                    if (DeactivatedWindow != null)
                    {
                        DeactivatedWindow();
                    }
                }
            }
        }
    }
}
