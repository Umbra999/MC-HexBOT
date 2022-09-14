using MCHexBOT.Core;
using MCHexBOT.Utils;

namespace MCHexBOT
{
    internal class Main
    {
        public static List<MinecraftClient> Clients = new();
        public static void Init()
        {
            CreateBots();
            RunGUI();
        }

        private static async void CreateBots()
        {
            APIClient Client = new();

            if (await Client.Login("XBL3.0 x=7114187592185725983;eyJlbmMiOiJBMTI4Q0JDLUhTMjU2IiwiYWxnIjoiUlNBLU9BRVAiLCJjdHkiOiJKV1QiLCJ6aXAiOiJERUYiLCJ4NXQiOiJzYVkzV1ZoQzdnMmsxRW9FU0Jncm9Ob2l3MVEifQ.RWwR0lP0n5WWuJRID3Q4xZvuoMfak0ihkPAx23vtxVzTxyE9UIy-E9pIrpsM4DqoNYzHUBEAXiuWS7CmenFyCiLbcHZNrw3xBkDzjzt0vM0Bquq2DO1d5l-HA_zG4pWvfbRYVR9i3Zq6aImEP6Xgd6LO7_nQM_CO1mHZzQL2M0Xxj5hvO3YHKpJyhuhtCOb9wWtKejrDtDTCLz9LjGn1HzIjkQonWGD5NmbcIE_4KoG4Rg8eW8xndx68WOfUXVxQrU51sl5N4KEySNxi2nQDW2EneXk9lfc8dVDUfLxtktdPD3D0WHUhzmMXXzyzPCfA3XxDSCbGFuQqtF9mHJFYbA.UGTzujogGgtmYlDwoiSsiA.qxm1q2kHQBZj_5pLTFj8RU7FZjRte8Mgya7yx2oznPKGzuocYgueO1LKcwiu0Yn4_fvl5jBk7r6vHW1zkFcIyANA43lzRqUdUBPrUdpl9_OhUYYYlYJfFW6bcLe5BMnw0XH8s3fhFJ_ESi54ZxmurqSTNXDRMf_QBUP4NMZNb7Z3wbv6hVVoVIxLVV1AtxKlwlHqy-vcQhQCe1lM1ju02LoNCWmiz_LyRcBDZ81YtFcG1z_KqhGc77AhGfVJmtRYIAM5v2IbaAMiwqEdadsSsZ_NvYudcXansoWN3y0aQmmfL5SyGG_HX-c0SgT298bHQmOpTwNvgbIiiWwdQ0joNIpjFt_PI0QGF7MH7yDP3hTioGJEtq7xnToAou9e2Qr6vXN0iSpEfwQ_ZfLqctfF2P3oMa9aAMvzETou8WDbyXmbWbtrA8yZCYYKqnuSsQXQVsWLyroz-obkxZt6QaOvMdHt7nrjd72OzM8EiSf8v0a1oEFKzpRkjFxnctIwBUbYnRnc_r6Jlt03wmpq3YE0QFvAL7SkJadiTsMz-Lxq4TqQs0DFEByagQmqYeElsU4LUBfJLMaoCDEhBvKcawqVwxFp-yL-J9jO_q_v5b8xYYUL8tGzXkmch5F-tWHaE9hQFckAjgWwH2FAq7R_kOu7dWeuFNVnQ3DF95NnRBMgldvghN8qek2pAThMnBCrDO3Erf0steE3w4tcYIQkb46HeItJt0tTgLJpYgsr7_8Kyraog7io0X8jY6To4t7hh2_890AH2r-klQz2P87sOym5RbfeDAC70E10XOg3MzZ7HTH38gvjrEC50W3GuBqruK4DjHKywHU4wMETyT7pVTRNkC-ak9gn8rpzzEpPZXgGZwlPwq54VkX6SEhTWxGiCBqfvyeUs16kYj5351zU9lH5r3x_cSzwVmgnPqmg90WoXK7MW-ixRtzuZdeUpJ_SIti6v_RnIYWwWJCL4aIfHbpWOKqOCMl2wrav1mUaQIC4b5T_kpBBqILkWpYmxQJU483vMcMkVEZEoD-fcX5iDxz467RGX0NVS67ATKQjAAdFsIb6CgbtB1nbbN7YKHFzkGppk2D28GN4StCyPftd0Gv1mBSY0m_opMc9fnRsHDXkeQALNQZ2KexssbWD0eXv7ab0jJbIpjyinog7BVOR3iypxHx-UpJuCSBmBsIxUXEograyH_gzXXPCc2FMLyeePefULELgCEMhA82jNh52IRvIFN6e02icZaW_m-rkLd9CSOlQj7khY6jn9XOFXYW4d_oTTL4TJPe9IBL10KuiNzWT4Uut0ok8W6wktDOKOCzPV6tueLxt9Ob2jNbyhF6vdTVtsyMU0ARRI2CtnSKSPCU2DInuk0tZfsrJENYUI1YvPvzmpk8p67SrhLH72cbQLGuDtWfOcdhe9a0S5y_RYO_WF_8A4Cm2JIimJRQvzAhlY06ghqw0yqKX8JzXV36aPXPbmNqtAG8DpJu9Gd9Vf1o1WUMcb_qvKCFkP6MU76TzYLrey4Eq4Bq_xrWE2LdscwjtIaTO8yZUMLGCQYGnAsSo2_7tSXdfVu-YJ7q4M4TFfLg9d8_KYwFAG9weNYUZ7rwL.KtBrQWbIJdxa28PiYv0QgA"))
            {
                Clients.Add(new MinecraftClient(Client));
            }
        }

        private static void RunGUI()
        {
            for (; ; )
            {
                Logger.LogImportant("-----------------");
                Logger.LogImportant("J [IP:PORT] - Join a Server");
                Logger.LogImportant("L - Leave a Server");

                string input = Console.ReadLine();
                new Thread(() => { HandleInput(input); Thread.CurrentThread.IsBackground = true; }).Start();
            }
        }

        private static void HandleInput(string input)
        {
            string InputStart = input.Split(' ')[0];
            switch (InputStart.ToLower())
            {
                case "j":
                    string Server = input.Substring(2);
                    foreach (MinecraftClient Client in Clients)
                    {
                        Client.Connect("1.18", Server.Split(':')[0], Convert.ToInt32(Server.Split(':')[1]));
                    }
                    break;

                case "l":
                    foreach (MinecraftClient Client in Clients)
                    {
                        Client.Disconnect();
                    }
                    break;
            }
        }
    }
}
