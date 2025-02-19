bool DelResult = DeleteRelatedRecords(mn, yr, VCode, active_wo_no, location_code);

if (DelResult)
{
    // Remove Deleted Rows from DataSet
    for (int i = PageRecordDataSet.Tables[0].Rows.Count - 1; i >= 0; i--)
    {
        if (PageRecordDataSet.Tables[0].Rows[i].RowState == DataRowState.Deleted)
        {
            PageRecordDataSet.Tables[0].Rows[i].Delete();
        }
    }

    // Accept changes after deleting rows
    PageRecordDataSet.AcceptChanges();

    // Optional: Reload the dataset from the database for accuracy
    LoadPageRecordDataSet();
}
