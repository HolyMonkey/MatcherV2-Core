using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Model;
using UnityEngine.SceneManagement;
using Matcher.ExpressionGenerator;

namespace Assets.Scripts
{
    class SessionActivator
    {
        private Session _session;
        public static Session s_current;
        public static Session Current
        {
            get
            {
#if UNITY_EDITOR
                if (s_current == null)
                {
                    return s_current = GetSampleSession();
                }
#endif
                return s_current;
            }
        }

        public SessionActivator(Session session)
        {
            _session = session;
        }

        public void Run()
        {
            s_current = _session;
            SceneManager.LoadScene("BattleScreen", LoadSceneMode.Single);
        }

        private static Session GetSampleSession()
        {
            return new Session(new Stage(new Mode(new Range(0, 10), new Range(0, 10), 100)),
                               new Stage(new Mode(new Range(0, 10), new Range(0, 10), 100)))
            {

            };
        }
    }
}
