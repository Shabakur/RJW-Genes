﻿using HarmonyLib;
using rjw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace RJW_Genes
{
    public class Gene_Aphrodisiac_Pheromones : Gene
    {   
        public override void Tick()
        {
            base.Tick();
            if (this.pawn.IsHashIntervalTick(2500))
            {
                foreach (Pawn pawn in this.AffectedPawns(this.pawn.Position, this.pawn.Map))
                { 
                    this.InduceAphrodisiac(pawn);
                }
            }
        }

        private IEnumerable<Pawn> AffectedPawns(IntVec3 pos, Map map)
        {
            foreach (Pawn pawn in map.mapPawns.AllPawns)
            {
                if (pos.DistanceTo(pawn.Position) < 5)
                {
                    yield return pawn;
                }
            }
            //IEnumerator<Pawn> enumerator = null;
            yield break;
        }

        private void InduceAphrodisiac(Pawn pawn)
        {
            Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Aphrodisiac_Pheromone);
            if (hediff != null)
            {
                hediff.Severity = 1f;
            }
            else
            {
                pawn.health.AddHediff(HediffDefOf.Aphrodisiac_Pheromone);
            }
        }
    }
}
