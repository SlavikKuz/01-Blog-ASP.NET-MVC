using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TeleDronBot.Commons;
using TeleDronBot.Interfaces;
using TeleDronBot.Repository;

namespace TeleDronBot.DTO
{
    class UserDTO
    {
        private bool disposed = false;

        #region Properties
        [Key]
        public long ChatId { get; set; }
        public string Login { get; set; }
        public string FIO { get; set; }
        public StepDTO step { get; set; }
        public string Phone { get; set; }

        public int IsRegister { get; set; }
        public int PilotPrivilage { get; set; }
        public int BusinessPrivilage { get; set; }

        public ICollection<ProposalDTO> proposals { get; set; }
        public UserDTO()
        {
            proposals = new List<ProposalDTO>();
        }

    #endregion

    public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #region private and protected methods

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Dispose();
                }
                disposed = true;
            }
        }

        ~UserDTO() =>
            Dispose(false);

        #endregion
    }
}
