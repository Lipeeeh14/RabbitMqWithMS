namespace Producer.Model
{
	public class Test
	{
        public string Message { get; set; }
		public bool Sucesso { get; set; }

		public Test(string message, bool sucesso)
		{
			Message = message;
			Sucesso = sucesso;
		}
	}
}
