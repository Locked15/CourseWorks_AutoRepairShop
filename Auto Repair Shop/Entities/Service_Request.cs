//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Auto_Repair_Shop.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Service_Request
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]

        public Service_Request()
        {
            this.Parts_To_Request = new HashSet<Parts_To_Request>();
        }
    
        public int Id { get; set; }
        public int Vehicle_Id { get; set; }
        public int Requester_Id { get; set; }
        public Nullable<System.DateTime> Request_Date { get; set; }
        public Nullable<System.DateTime> Request_Approx_Complete { get; set; }
        public int Service_Type_Id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Parts_To_Request> Parts_To_Request { get; set; }
        public virtual Person Person { get; set; }
        public virtual Vehicle Vehicle { get; set; }
        public virtual Service_Type Service_Type { get; set; }

        /// <summary>
        /// Рассчитывает стоимость обслуживания по используемым деталям и их количеству.
        /// </summary>
        /// <returns>Итоговая стоимость.</returns>
        public decimal calculateBasePrice() {
            decimal price = 0;

            foreach (var item in Parts_To_Request) {
                price += item.Part.Part_Price * item.Count;
            }

            return price;
        }

        /// <summary>
        /// Рассчитывает итоговую стоимость обслуживания.
        /// </summary>
        /// <returns></returns>
        public decimal calculateTotalPrice() {
            decimal price;

            try {
                price = (calculateBasePrice() + Service_Type.Price) * Vehicle.calculateClassModifier();
            } catch {
                price = new Random().Next(1000, 250000);
            }

            return price;
        }
    }
}
