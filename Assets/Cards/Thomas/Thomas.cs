﻿using System;
using Kamikaze.Backend;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kamikaze
{
	public class Thomas : UnitCard
	{
		public Thomas(Player owner, Player opponent, IEnumerable container, Frontend_Old.Card front, GameController game, GameActions actions) 
			: base(owner, opponent, container, front, game, actions)
		{
			Attack = new AttackStat(AttackMode.Frontal, 3);
			Movement = new MovementStat(MovementMode.March, 10);

			Abilites = new List<Ability>
			{
				new Move (this, Movement),
				new Attack (this, Attack),
				new SayChooChooAbility(this)
			};

			TriggerEffects = new List<TriggerEffect>
			{
				new PunchFascistsEffect(this),
			};
		}

		public class SayChooChooAbility : Ability
		{
			public SayChooChooAbility(Card effectOwnerCard) : base(effectOwnerCard) { }

			public override bool Condition() => true;
			
			public override async Task Body()
			{
				await Actions.Damage(card as IHealthStatHolder);
				await Actions.DisplayMessage("Thomas: ChooChoo, motherfucker");
			}
		}
		
		public class PunchFascistsEffect : TriggerEffect<GameEvents.OnCardDamaged>
		{
			public PunchFascistsEffect(Card effectOwnerCard) : base(effectOwnerCard) {}
			
			public override bool Condition()
			{
				return context.damagedCard.owner != effectOwnerCard.owner;
			}
			
			public override async Task Body()
			{
				await Actions.DisplayMessage("Thomas: E apanhou pouco, facista de merda");
			}
		}
	}	
}