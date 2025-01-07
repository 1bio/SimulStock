namespace GameServer;
using System;

class Constants
{
    public const int TICKS_PER_SEC = 30; // 초당 Tick의 수, 서버가 초당 30번 업데이트
    public const int MS_PER_TICK = 1000 / TICKS_PER_SEC; // 틱 간격, 1000 / 30 = 33ms마다 한 번의 Tick이 발생
}