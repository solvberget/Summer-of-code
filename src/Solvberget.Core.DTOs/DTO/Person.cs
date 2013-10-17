namespace Solvberget.Domain.DTO
{
    public class Person
    {
        public string Name { get; set; }
        public string LivingYears { get; set; }
        public string Role { get; set; }
        public string Nationality { get; set; }
        public string ReferredWork { get; set; }

        public void InvertName(string Name)
        {
            if (Name == null) 
                return;

            var nameTemp = Name;
            var lastNameFirstName = nameTemp.Split(',');
            if (lastNameFirstName.Length > 1)
            {
                var firstname = lastNameFirstName[1].Trim();
                var lastname = lastNameFirstName[0].Trim();
                nameTemp = firstname + " " + lastname;
                this.Name = nameTemp;
            }
        }
    }
}
