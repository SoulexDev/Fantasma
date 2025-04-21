using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Fantasma.Framework
{
    public class DebugLog
    {
        private static DebugLog m_instance;
        private static Dictionary<string, ConsoleLog> m_consoleLogs = new Dictionary<string, ConsoleLog>();

        private static int rewriteWait = 0;

        public static DebugLog SetTextColor(ConsoleColor textColor)
        {
            Console.ForegroundColor = textColor;
            return m_instance;
        }
        public static void Log(string log, bool rewriteConsole = true)
        {
            if (m_consoleLogs.ContainsKey(log))
            {
                ConsoleLog consoleLog = m_consoleLogs[log];
                if (consoleLog.m_textColor == Console.ForegroundColor)
                {
                    consoleLog.m_logCount++;

                    m_consoleLogs[log] = consoleLog;

                    if (rewriteConsole)
                        RewriteConsole();
                }
                else
                {
                    m_consoleLogs.Add(log, new ConsoleLog(Console.ForegroundColor));
                    Console.WriteLine(log);
                }
            }
            else
            {
                m_consoleLogs.Add(log, new ConsoleLog(Console.ForegroundColor));
                Console.WriteLine(log);
            }
        }
        private static void RewriteConsole()
        {
            rewriteWait++;

            if (rewriteWait < 15)
                return;
            else
                rewriteWait = 0;

            Console.Clear();
            foreach (var log in m_consoleLogs)
            {
                string finalLog = $"{log.Key} {log.Value.m_logCount}";
                Console.ForegroundColor = log.Value.m_textColor;

                Console.WriteLine(finalLog);
            }
        }
    }
    public struct ConsoleLog
    {
        public int m_logCount;
        public ConsoleColor m_textColor;

        public ConsoleLog(ConsoleColor color)
        {
            m_logCount = 0;
            m_textColor = color;
        }
    }
}
