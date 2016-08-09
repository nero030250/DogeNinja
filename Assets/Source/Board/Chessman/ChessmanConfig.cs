using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ChessmanType {
	Empty = 0,
	Monster = 1,		// 普通怪物
	Bomb = 2,			// 炸弹
	Bomber = 3,			// 怪物+炸弹
	Weapon = 4,			// 武器怪物
	Shield = 5,			// 带盾怪物, 能挡住忍者, 被攻击掉盾变成普通怪物
	Bomb_Shield = 6,		// 炸弹+盾牌
	Weapon_Shield = 7,	// 武器+盾牌

	Shooter = 10,
	Shit = 11,

	Doge_Ninja = 21,
}

public class ChessmanConfig{
	public string Name;// { get; private set; }
	public ChessmanType Type;// { get; private set; }
	public int Score;// { get; private set; }
	public List<int> FirstWeight;// { get; private set; }
	public List<int> SecondWeight;// { get; private set; }
}