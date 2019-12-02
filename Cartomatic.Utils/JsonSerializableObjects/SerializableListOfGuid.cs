using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartomatic.Utils.JsonSerializableObjects
{
    /// <summary>
    /// explicit implementation of a serializable list of guid
    /// </summary>
    public class SerializableListOfGuid : SerializableList<Guid>
    {
        /// <summary>
        /// hide the base List Capacity property so EF does not force pushes it into db model!
        /// </summary>
        private new int Capacity
        {
            get => base.Capacity;
            set => base.Capacity = value;
        }
    }
}
