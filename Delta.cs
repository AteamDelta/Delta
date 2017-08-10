using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Ateam
{
    public class Delta : BaseBattleAISystem
    {
        Dictionary<int, CharacterModel.Data> Players;
        Dictionary<int, CharacterModel.Data> Enemys;
        Dictionary<int, Common.MOVE_TYPE> Dir;
        Dictionary<int, Vector2> OldPos;

        //---------------------------------------------------
        // InitializeAI
        //---------------------------------------------------
        override public void InitializeAI()
        {
            Players = new Dictionary<int, CharacterModel.Data>();
            Enemys = new Dictionary<int, CharacterModel.Data>();
            Dir = new Dictionary<int, Common.MOVE_TYPE>();
            OldPos = new Dictionary<int, Vector2>();

            //データ取得
            var PlayerDataList = GetTeamCharacterDataList(TEAM_TYPE.PLAYER);
            var EnemyDataList = GetTeamCharacterDataList(TEAM_TYPE.ENEMY);

            foreach (var player in PlayerDataList)
            {
                Players.Add(player.ActorId, player);
                Dir[player.ActorId] = Common.MOVE_TYPE.NONE;
                OldPos[player.ActorId] = player.BlockPos;
            }
            foreach (var enemy in EnemyDataList)
            {
                Enemys.Add(enemy.ActorId, enemy);
                Dir[enemy.ActorId] = Common.MOVE_TYPE.NONE;
                OldPos[enemy.ActorId] = enemy.BlockPos;
            }
        }

        //---------------------------------------------------
        // UpdateAI
        //---------------------------------------------------
        override public void UpdateAI()
        {
            var PlayerDataList = GetTeamCharacterDataList(TEAM_TYPE.PLAYER);
            var EnemyDataList = GetTeamCharacterDataList(TEAM_TYPE.ENEMY);
            int[,] StageData = GetStageData();

            //データの更新
            foreach (var data in PlayerDataList)
            {
                Players[data.ActorId] = data;
                var oldPos = OldPos[data.ActorId];
                if (oldPos != data.BlockPos)
                {
                    //方向の更新
                    if (data.BlockPos.x > oldPos.x)
                    {
                        Dir[data.ActorId] = Common.MOVE_TYPE.RIGHT;
                    }
                    else if (data.BlockPos.x < oldPos.x)
                    {
                        Dir[data.ActorId] = Common.MOVE_TYPE.LEFT;
                    }
                    else if (data.BlockPos.y > oldPos.y)
                    {
                        Dir[data.ActorId] = Common.MOVE_TYPE.UP;
                    }
                    else if (data.BlockPos.y < oldPos.y)
                    {
                        Dir[data.ActorId] = Common.MOVE_TYPE.DOWN;
                    }
                    OldPos[data.ActorId] = data.BlockPos;
                }
            }

            foreach (var data in EnemyDataList)
            {
                Enemys[data.ActorId] = data;
                var oldPos = OldPos[data.ActorId];
                if (oldPos != data.BlockPos)
                {
                    //方向の更新
                    if (data.BlockPos.x > oldPos.x)
                    {
                        Dir[data.ActorId] = Common.MOVE_TYPE.RIGHT;
                    }
                    else if (data.BlockPos.x < oldPos.x)
                    {
                        Dir[data.ActorId] = Common.MOVE_TYPE.LEFT;
                    }
                    else if (data.BlockPos.y > oldPos.y)
                    {
                        Dir[data.ActorId] = Common.MOVE_TYPE.UP;
                    }
                    else if (data.BlockPos.y < oldPos.y)
                    {
                        Dir[data.ActorId] = Common.MOVE_TYPE.DOWN;
                    }
                    OldPos[data.ActorId] = data.BlockPos;
                }
            }

            //プレイヤー処理
            foreach (var player in PlayerDataList)
            {
                //移動中か
                //if (player.isMoveEnable == false)
                //{
                //    continue;
                //}

                //攻撃
                Common.MOVE_TYPE attackDir = Common.MOVE_TYPE.NONE;
                float Length = 20.0f;
                bool bAttack = false;
                int DangerEnemy = -1;

                foreach (var enemy in EnemyDataList)
                {
                    //重なり
                    if (player.BlockPos == enemy.BlockPos)
                    {
                        continue;
                    }

                    //直線上にいるか
                    //上下
                    if (player.BlockPos.x == enemy.BlockPos.x)
                    {
                        //上に敵
                        if (player.BlockPos.y < enemy.BlockPos.y)
                        {
                            //ブロック判定

                            //向き合っているか
                            if( Dir[enemy.ActorId] == Common.MOVE_TYPE.DOWN)
                            {
                                DangerEnemy = enemy.ActorId;
                                break;
                            }

                            if (Dir[player.ActorId] == Common.MOVE_TYPE.UP)
                            {
                                float len = enemy.BlockPos.y - player.BlockPos.y;
                                if (len < Length)
                                {
                                    Length = len;
                                    attackDir = Common.MOVE_TYPE.UP;
                                }
                            }
                        }

                        //下に敵
                        if (player.BlockPos.y > enemy.BlockPos.y)
                        {

                            //向き合っているか
                            if (Dir[enemy.ActorId] == Common.MOVE_TYPE.UP)
                            {
                                DangerEnemy = enemy.ActorId;
                                break;
                            }

                            if (Dir[player.ActorId] == Common.MOVE_TYPE.DOWN)
                            {
                                float len = player.BlockPos.y - enemy.BlockPos.y;
                                if (len < Length)
                                {
                                    Length = len;
                                    attackDir = Common.MOVE_TYPE.DOWN;
                                }
                            }
                        }
                    }

                    //左右
                    if (player.BlockPos.y == enemy.BlockPos.y)
                    {
                        //右に敵
                        if (player.BlockPos.x < enemy.BlockPos.x)
                        {

                            //向き合っているか
                            if (Dir[enemy.ActorId] == Common.MOVE_TYPE.LEFT)
                            {
                                DangerEnemy = enemy.ActorId;
                                break;
                            }

                            if (Dir[player.ActorId] == Common.MOVE_TYPE.RIGHT)
                            {
                                float len = enemy.BlockPos.x - player.BlockPos.x;
                                if (len < Length)
                                {
                                    Length = len;
                                    attackDir = Common.MOVE_TYPE.RIGHT;
                                }
                            }
                        }

                        //左に敵
                        if (player.BlockPos.x > enemy.BlockPos.x)
                        {

                            //向き合っているか
                            if (Dir[enemy.ActorId] == Common.MOVE_TYPE.RIGHT)
                            {
                                DangerEnemy = enemy.ActorId;
                                break;
                            }

                            if (Dir[player.ActorId] == Common.MOVE_TYPE.LEFT)
                            {
                                float len = player.BlockPos.x - enemy.BlockPos.x;
                                if (len < Length)
                                {
                                    Length = len;
                                    attackDir = Common.MOVE_TYPE.LEFT;
                                }
                            }
                        }
                    }
                }   //end foreach enemy

                //攻撃
                if (attackDir != Common.MOVE_TYPE.NONE || DangerEnemy == -1)
                {
                    bool CanShot = false;
                    //近距離
                    if( Length < 3)
                    {
                        CanShot = Action(player.ActorId, Define.Battle.ACTION_TYPE.ATTACK_SHORT);
                    }
                    //中距離
                    if ( CanShot == false && Length < 8)
                    {
                        CanShot = Action(player.ActorId, Define.Battle.ACTION_TYPE.ATTACK_MIDDLE);
                    }
                    //長距離
                    if( CanShot == false)
                    {
                        CanShot = Action(player.ActorId, Define.Battle.ACTION_TYPE.ATTACK_LONG);
                    }
                    bAttack = CanShot;
                }

                //移動
                if(bAttack == false)
                {
                    //にげる
                    if(DangerEnemy != -1)
                    {

                    }


                    else
                    {

                    }

                    Vector2 enemyPos = new Vector2();       //移動対象の位置
                    foreach( var enemy in EnemyDataList)
                    {
                        if( enemy.Hp > 0)
                        {
                            enemyPos = enemy.BlockPos;
                            break;
                        }
                    }

                    //最短距離
                    if (player.BlockPos.y < enemyPos.y &&
                        StageData[(int)player.BlockPos.y + 1, (int)player.BlockPos.x] == 0)
                    {
                        Move(player.ActorId, Common.MOVE_TYPE.UP);
                    }
                    else if (player.BlockPos.y > enemyPos.y &&
                        StageData[(int)player.BlockPos.y - 1, (int)player.BlockPos.x] == 0)
                    {
                        Move(player.ActorId, Common.MOVE_TYPE.DOWN);

                    }

                    else if (player.BlockPos.x < enemyPos.x &&
                        StageData[(int)player.BlockPos.y, (int)player.BlockPos.x + 1] == 0)
                    {
                        Move(player.ActorId, Common.MOVE_TYPE.RIGHT);
                    }

                    else if (player.BlockPos.x > enemyPos.x &&
                        StageData[(int)player.BlockPos.y, (int)player.BlockPos.x - 1] == 0)
                    {
                        Move(player.ActorId, Common.MOVE_TYPE.LEFT);
                    }

                    //遠回り
                    else if (StageData[(int)player.BlockPos.y + 1, (int)player.BlockPos.x] == 0)
                    {
                        Move(player.ActorId, Common.MOVE_TYPE.UP);
                    }
                    else if (StageData[(int)player.BlockPos.y - 1, (int)player.BlockPos.x] == 0)
                    {
                        Move(player.ActorId, Common.MOVE_TYPE.DOWN);
                    }

                    else if (StageData[(int)player.BlockPos.y, (int)player.BlockPos.x + 1] == 0)
                    {
                        Move(player.ActorId, Common.MOVE_TYPE.RIGHT);
                    }

                    else if (StageData[(int)player.BlockPos.y, (int)player.BlockPos.x - 1] == 0)
                    {
                        Move(player.ActorId, Common.MOVE_TYPE.LEFT);
                    }


                    //var rand = Random.Range(0, 4);
                    //if( rand == 0)
                    //{
                    //    Move(player.ActorId, Common.MOVE_TYPE.UP);
                    //}
                    //if (rand == 1)
                    //{
                    //    Move(player.ActorId, Common.MOVE_TYPE.DOWN);
                    //}
                    //if (rand == 2)
                    //{
                    //    Move(player.ActorId, Common.MOVE_TYPE.RIGHT);
                    //}
                    //if (rand == 3)
                    //{
                    //    Move(player.ActorId, Common.MOVE_TYPE.LEFT);
                    //}
                }
            }
        }

        //---------------------------------------------------
        // ItemSpawnCallback
        //---------------------------------------------------
        override public void ItemSpawnCallback(ItemSpawnData itemData)
        {

        }

        
    }
}