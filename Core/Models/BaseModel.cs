using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atf.Core.Models
{
    /// <summary>
    /// Base data model with an identifier.
    /// </summary>
    public abstract class BaseModel
    {
        public int Id { get; set; }
    }
}
