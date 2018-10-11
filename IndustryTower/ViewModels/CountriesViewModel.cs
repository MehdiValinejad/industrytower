using IndustryTower.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IndustryTower.DAL;
using System.Web.Mvc;
using IndustryTower.App_Start;

namespace IndustryTower.ViewModels
{

    public class CountriesViewModel
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        public SelectList countries
        {
            get
            {
                var countries = unitOfWork.CountstateRepository.Get(c => c.countryID == null)
                                                               .OrderBy(o => o.CultureStateName);
                if (ITTConfig.CurrentCultureIsNotEN)
                {
                    return new SelectList(countries, "stateID", "stateName");
                }
                else return new SelectList(countries, "stateID", "stateNameEN");
            }
        }

        public SelectList counts(CountState state)
        {
            var countries = unitOfWork.CountstateRepository.Get(c => c.countryID == null)
                                                           .OrderBy(o => o.CultureStateName);
            if (ITTConfig.CurrentCultureIsNotEN)
            {
                return new SelectList(countries, "stateID", "stateName", state.country.stateID);
            }
            else return new SelectList(countries, "stateID", "stateNameEN", state.country.stateID);
        }

        public SelectList states(CountState state)
        {
            var otherStates = unitOfWork.CountstateRepository.Get(c => c.countryID == state.countryID)
                                                             .OrderBy(o => o.CultureStateName);
            if (ITTConfig.CurrentCultureIsNotEN)
            {
                return new SelectList(otherStates, "stateID", "stateName", state.stateID);
            }
            else return new SelectList(otherStates, "stateID", "stateNameEN", state.stateID);
        }


    }
}