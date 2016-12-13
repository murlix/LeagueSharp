﻿namespace Flowers_Riven_Reborn.Manager.Events.Games.Mode
{
    using Spells;
    using myCommon;
    using LeagueSharp;
    using LeagueSharp.Common;
    using Orbwalking = myCommon.Orbwalking;

    internal class Combo : Logic
    {
        internal static void Init()
        {
            var target = TargetSelector.GetSelectedTarget() ??
                TargetSelector.GetTarget(900, TargetSelector.DamageType.Physical);

            if (target.Check(900f))
            {
                if (Menu.GetBool("ComboIgnite") && Ignite != SpellSlot.Unknown && Ignite.IsReady() &&
                    SpellManager.GetComboDamage(target) > target.Health)
                {
                    Me.Spellbook.CastSpell(Ignite, target);
                }

                if (Menu.GetBool("ComboW") && W.IsReady() &&
                    target.IsValidTarget(W.Range) && !target.HasBuffOfType(BuffType.SpellShield) &&
                    (!Q.IsReady() || qStack != 0))
                {
                    W.Cast(true);
                }

                if (Menu.GetBool("ComboE") && E.IsReady() && Me.CanMoveMent() && target.DistanceToPlayer() <= 650 &&
                    target.DistanceToPlayer() > Orbwalking.GetRealAutoAttackRange(Me) + 100)
                {
                    if (target.DistanceToPlayer() <= E.Range + (Q.IsReady() && qStack == 0? Q.Range:0))
                    {
                        E.Cast(target.Position, true);
                    }
                    else if (target.DistanceToPlayer() <= E.Range + (W.IsReady() ? W.Range : 0))
                    {
                        E.Cast(target.Position, true);
                    }
                    else if (!Q.IsReady() && !W.IsReady() && target.DistanceToPlayer() < E.Range + Me.AttackRange)
                    {
                        E.Cast(target.Position, true);
                    }
                }

                if (Menu.GetBool("ComboQ") && Q.IsReady() && !Me.IsDashing() && Me.CanMoveMent() &&
                    target.DistanceToPlayer() <= Q.Range + Orbwalking.GetRealAutoAttackRange(Me) &&
                    !Orbwalking.InAutoAttackRange(target) && Utils.TickCount - lastQTime > 500)
                {
                    SpellManager.CastQ(target);
                }

                if (Menu.GetBool("ComboR") && R.IsReady())
                {
                    if (Menu.GetKey("R1Combo") && R.Instance.Name == "RivenFengShuiEngine" && !E.IsReady())
                    {
                        if (target.DistanceToPlayer() < 500 && Me.CountEnemiesInRange(500) >= 1)
                        {
                            R.Cast(true);
                        }
                    }
                    else if (R.Instance.Name == "RivenIzunaBlade")
                    {
                        SpellManager.R2Logic(target);
                    }
                }
            }
        }
    }
}