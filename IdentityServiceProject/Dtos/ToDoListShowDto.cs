namespace IdentityServiceProject.Dtos
{
    public class ToDoListShowDto: ToDoBase
    {
        public override string UserName { get; set; }

        public bool IsDone { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
