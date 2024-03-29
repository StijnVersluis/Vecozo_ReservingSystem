﻿using IntefaceLayer;
using IntefaceLayer.DTO;
using System;
using System.Collections.Generic;

namespace BusinessLayer
{
    public class WorkzoneContainer
    {
        private IWorkzoneContainer workzoneContainer;
        public WorkzoneContainer(IWorkzoneContainer workzoneContainer)
        {
            this.workzoneContainer = workzoneContainer;
        }

        public Workzone GetById(int id)
        {
            Workzone output = null;
            WorkzoneDTO result = workzoneContainer.GetById(id);

            if (result != null)
            {
                output = new Workzone(result);
            }

            return output;
        }

        public bool DeleteWorkzone(int id)
        {
            return workzoneContainer.DeleteWorkzone(id);
        }

        //public Workzone GetByDateAndId(int id, string date)
        //{
        //    Workzone output = null;
        //    WorkzoneDTO result = workzoneContainer.GetByDateAndId(id, date);

        //    if (result != null)
        //    {
        //        output = new Workzone(result);
        //    }

        //    return output;
        //}

        public List<Workzone> GetAll()
        {
            return workzoneContainer.GetAll().ConvertAll(x => new Workzone(x));
        }
        public List<Workzone> GetAllFromFloor(int id)
        {
            return workzoneContainer.GetAllFromFloor(id).ConvertAll(x => new Workzone(x));
        }
        //public List<Workzone> GetAllFromFloorWithDate(int id, string date)
        //{
        //    return workzoneContainer.GetAllFromFloorWithDate(id, date).ConvertAll(x => new Workzone(x));
        //}
        public bool Edit(Workzone workzone)
        {
            return workzoneContainer.Edit(workzone.toDTO());
        }

        public List<string> CheckEditRules(Workzone workzone, IWorkzone dal)
        {
            List<string> messages = new();

            if (string.IsNullOrEmpty(workzone.Name))
            {
                messages.Add("Voer alle velden in!");
            }

            if (workzone.Workspaces < 0)
            {
               messages.Add("Het aantal werplekken moet groter zijn dan 0");
            }

            if (workzone.HasReservations(dal))
            {
                messages.Add("U kunt geen werkblok wijzigen met actieve reseveringen.");
            }

            return messages;
        }
        public bool DeleteWorkzone(int id)
        {
            return workzoneContainer.DeleteWorkzone(id);
        }
    }
}
