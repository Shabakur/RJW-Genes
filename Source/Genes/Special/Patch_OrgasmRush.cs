﻿using HarmonyLib;
using rjw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace RJW_Genes
{
	
	[HarmonyPatch(typeof(SexUtility), nameof(SexUtility.SatisfyPersonal))]
	public static class Patch_OrgasmRush
	{

		private const float REST_INCREASE = 0.05f;
		private const float ORGASMS_NEEDED_FOR_SUPERCHARGE = 3.0f;

		public static void Postfix(SexProps props)
		{
			// ShortCuts: Exit Early if Pawn or Partner are null (can happen with Animals or Masturbation)
			if (props.pawn == null || !props.hasPartner())
				return;

			if (props.pawn.genes != null && props.pawn.genes.HasGene(GeneDefOf.rjw_genes_orgasm_rush))
            {

				// Pump up Wake-Ness
				if (props.pawn.needs.rest != null)
					props.pawn.needs.rest.CurLevel += REST_INCREASE;

				// Add or Update Hediff for Orgasm Rush
				Hediff rush = GetOrgasmRushHediff(props.pawn);
				float added_severity = props.orgasms / ORGASMS_NEEDED_FOR_SUPERCHARGE;
				rush.Severity += added_severity;
				// Severity should be capped to 1 by the XML logic
			}

		}

		/// <summary>
		/// Helps to get the Orgasm Rush Hediff of a Pawn. If it does not exist, one is added. 
		/// </summary>
		/// <param name="orgasmed">The pawn that had the orgasm, for which a hediff is looked up or created.</param>
		/// <returns></returns>
		public static Hediff GetOrgasmRushHediff(Pawn orgasmed)
		{
			Hediff orgasmRushHediff = orgasmed.health.hediffSet.GetFirstHediffOfDef(GeneDefOf.rjw_genes_orgasm_rush_hediff);
			if (orgasmRushHediff == null)
			{
				orgasmRushHediff = HediffMaker.MakeHediff(GeneDefOf.rjw_genes_orgasm_rush_hediff, orgasmed);
				orgasmRushHediff.Severity = 0;
				orgasmed.health.AddHediff(orgasmRushHediff);
			}
			return orgasmRushHediff;
		}
	}


}
