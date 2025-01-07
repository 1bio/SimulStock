namespace GameServer;
using System;

class Program
{
    private static bool isRunning = false; 

    static void Main(string[] args)
    {
        Console.Title = "Game Server";
        isRunning = true;
    
        Thread mainThread = new Thread(new ThreadStart(MainThread)); // 메인 스레드 생성
        mainThread.Start(); 

        Server.Start(50, 41795); // 서버 시작(최대 클라이언트 50명, 포트 41795);
    }

    // 서버 메인 스레드 
    private static void MainThread()
    {
        Console.WriteLine($"Main thread started. Running at {Constants.TICKS_PER_SEC} ticks per second.");
        DateTime _nextLoop = DateTime.Now; // 다음 업데이트가 이루어질 시간

        while (isRunning)
        {
            while (_nextLoop < DateTime.Now)
            {
                GameLogic.Update(); // 게임 상태 업데이트

                _nextLoop = _nextLoop.AddMilliseconds(Constants.MS_PER_TICK);
                
                // 다음 업데이트 시간이 도달하지 않았을 경우
                // 서버 절전, 메모리 누수 방지 
                if (_nextLoop > DateTime.Now)
                {
                    Thread.Sleep(_nextLoop - DateTime.Now);
                }
            }
        }
    }
}


