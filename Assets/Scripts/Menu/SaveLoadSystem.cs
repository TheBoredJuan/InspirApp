using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System.Threading.Tasks;

public class SaveLoadSystem : MonoBehaviour
{
    private FirebaseDatabase database;
    private DatabaseReference databaseReference;

    private void Start()
    {
        database = FirebaseDatabase.DefaultInstance;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }
    public void SavePatient(PatientModel dataPatient) {

        databaseReference.Child("patients").Child(dataPatient.username).SetRawJsonValueAsync(JsonUtility.ToJson(dataPatient));
    }

    public async Task<PatientModel> LoadPatient(string username) {
        var dataSnapshot = await database.GetReference("patients").Child(username).GetValueAsync();
        if (!dataSnapshot.Exists) {
            return null;
        }
        return JsonUtility.FromJson<PatientModel>(dataSnapshot.GetRawJsonValue());
    }

    public async Task<bool> PatientExists(string username) {
        var dataSnapshot = await databaseReference.Child("patients").Child(username).GetValueAsync();
        return dataSnapshot.Exists;
    }

    public async Task<PrescriptionModel> LoadPatientPrescription(string username) {
        var dataSnapshot = await database.GetReference("prescriptions").Child(username).GetValueAsync();
        if (!dataSnapshot.Exists)
        {
            PrescriptionModel defaultPrescription = PrescriptionModel.getDefaultPrescription(username);
            await databaseReference.Child("prescriptions").Child(username).SetRawJsonValueAsync(JsonUtility.ToJson(defaultPrescription));
            return defaultPrescription;
        }
        return JsonUtility.FromJson<PrescriptionModel>(dataSnapshot.GetRawJsonValue());
    }
}
