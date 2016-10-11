/*
 * User: cookc
 * Date: 27/11/2013
 * Time: 4:10 PM 
 */
using System;
using Redux.Game_Server;
using Redux.Structures;
using Redux.Enum;
using Redux.Database;
using Redux.Database.Domain;

namespace Redux.Items
{
	/// <summary>
	/// Handles item usage for [723700] MeteorScroll
	/// </summary>
    public class Item_723700: IItem
	{		
        public override void Run(Player _client, ConquerItem _item) 
        {
            if (_client.Level >= 140)
                return;
            if (!_client.Tasks.ContainsKey(TaskType.ExpBall))
                _client.Tasks.TryAdd(TaskType.ExpBall, new Task(_client.UID, TaskType.ExpBall, DateTime.Now.AddHours(24)));
            var task = _client.Tasks[TaskType.ExpBall];
            if (DateTime.Now > task.Expires)
            { task.Expires = DateTime.Now.AddHours(24); task.Count = 0; }
            if (task.Count >= Constants.EXP_BALL_LIMIT)
                _client.SendMessage("You have used too many exp balls. Please wait until tomorrow");
            else
            {
                task.Count++;
                _client.DeleteItem(_item);
                _client.GainExpBall();
            }
		}
	}
}
