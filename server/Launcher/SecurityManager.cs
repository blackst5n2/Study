using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;

namespace Launcher
{
    public class SecurityManager
    {
        public bool VerifyExeIntegrity(string exePath, string expectedHash)
        {
            using (var sha256 = SHA256.Create())
            using (var stream = File.OpenRead(exePath))
            {
                byte[] hash = sha256.ComputeHash(stream);
                string actualHash = BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();
                return actualHash == expectedHash.ToLowerInvariant();
            }
        }

        public bool IsSigned(string exePath)
        {
            // 실제 코드사인 검증은 WinVerifyTrust 등 네이티브 API 필요
            // 여기서는 구조만 제공
            return true;
        }

        public bool IsLaunchedByLauncher(string expectedLauncherName)
        {
            try
            {
                using (Process current = Process.GetCurrentProcess())
                using (Process parent = GetParentProcess(current))
                {
                    if (parent == null) return false;
                    string parentName = parent.MainModule.ModuleName;
                    return string.Equals(parentName, expectedLauncherName, StringComparison.OrdinalIgnoreCase);
                }
            }
            catch
            {
                return false;
            }
        }

        private Process GetParentProcess(Process process)
        {
            try
            {
                int parentPid = 0;
                using (var searcher = new System.Management.ManagementObjectSearcher($"SELECT ParentProcessId FROM Win32_Process WHERE ProcessId = {process.Id}"))
                {
                    foreach (var obj in searcher.Get())
                    {
                        parentPid = Convert.ToInt32(obj["ParentProcessId"]);
                        break;
                    }
                }
                return parentPid > 0 ? Process.GetProcessById(parentPid) : null;
            }
            catch
            {
                return null;
            }
        }
    }
}
