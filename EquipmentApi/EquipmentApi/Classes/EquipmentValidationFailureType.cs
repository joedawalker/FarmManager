namespace EquipmentApi.Classes
{
	public enum EquipmentValidationFailureType
	{
		MissingEquipmentItem = 0,
		MissingEquipmentId = 1,
		InvalidEquipmentId = 2,
		MissingEquipmentName = 3,
		GenericErrorOccurred = 4
	}
}