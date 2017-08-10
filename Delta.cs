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

            //�f�[�^�擾
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

            //�f�[�^�̍X�V
            foreach (var data in PlayerDataList)
            {
                Players[data.ActorId] = data;
                var oldPos = OldPos[data.ActorId];
                if (oldPos != data.BlockPos)
                {
                    //�����̍X�V
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
                    //�����̍X�V
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

            //�v���C���[����
            foreach (var player in PlayerDataList)
            {
                //�ړ�����
                //if (player.isMoveEnable == false)
                //{
                //    continue;
                //}

                //�U��
                Common.MOVE_TYPE attackDir = Common.MOVE_TYPE.NONE;
                float Length = 20.0f;
                bool bAttack = false;
                int DangerEnemy = -1;

                foreach (var enemy in EnemyDataList)
                {
                    //�d�Ȃ�
                    if (player.BlockPos == enemy.BlockPos)
                    {
                        continue;
                    }

                    //������ɂ��邩
                    //�㉺
                    if (player.BlockPos.x == enemy.BlockPos.x)
                    {
                        //��ɓG
                        if (player.BlockPos.y < enemy.BlockPos.y)
                        {
                            //�u���b�N����

                            //���������Ă��邩
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

                        //���ɓG
                        if (player.BlockPos.y > enemy.BlockPos.y)
                        {

                            //���������Ă��邩
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

                    //���E
                    if (player.BlockPos.y == enemy.BlockPos.y)
                    {
                        //�E�ɓG
                        if (player.BlockPos.x < enemy.BlockPos.x)
                        {

                            //���������Ă��邩
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

                        //���ɓG
                        if (player.BlockPos.x > enemy.BlockPos.x)
                        {

                            //���������Ă��邩
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

                //�U��
                if (attackDir != Common.MOVE_TYPE.NONE || DangerEnemy == -1)
                {
                    bool CanShot = false;
                    //�ߋ���
                    if( Length < 3)
                    {
                        CanShot = Action(player.ActorId, Define.Battle.ACTION_TYPE.ATTACK_SHORT);
                    }
                    //������
                    if ( CanShot == false && Length < 8)
                    {
                        CanShot = Action(player.ActorId, Define.Battle.ACTION_TYPE.ATTACK_MIDDLE);
                    }
                    //������
                    if( CanShot == false)
                    {
                        CanShot = Action(player.ActorId, Define.Battle.ACTION_TYPE.ATTACK_LONG);
                    }
                    bAttack = CanShot;
                }

                //�ړ�
                if(bAttack == false)
                {
                    //�ɂ���
                    if(DangerEnemy != -1)
                    {

                    }


                    else
                    {

                    }

                    Vector2 enemyPos = new Vector2();       //�ړ��Ώۂ̈ʒu
                    foreach( var enemy in EnemyDataList)
                    {
                        if( enemy.Hp > 0)
                        {
                            enemyPos = enemy.BlockPos;
                            break;
                        }
                    }

                    //�ŒZ����
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

                    //�����
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