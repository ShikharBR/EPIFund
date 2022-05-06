using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inview.Epi.EpiFund.Domain.Entity
{
    public class AssetMessage
    {
        public AssetMessage()
        {
        }

        [Key]
        public virtual int AssetMessageId { get; set; }
        public virtual Guid AssetId { get; set; }
        public virtual string Message { get; set; }
        public virtual int UserId { get; set; }
        public virtual string UserName { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual bool HasBeenRead { get; set; }
    }
}
