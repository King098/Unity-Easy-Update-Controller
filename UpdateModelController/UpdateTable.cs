using System.Collections;
using System.Collections.Generic;

public enum UpdateType
{
	Table = 1,
	Model = 2,
	Dress = 3,
}

public class UpdateInfo
{
	public string name;
	public string android_link;
	public string link;
	public UpdateType type;
	public string version;

	public UpdateInfo(string[] temp)
	{
		int offset = 0;
		name = temp[offset].ToString();offset++;
		android_link = temp[offset].ToString();offset++;
		link = temp[offset].ToString();offset++;
		type = (UpdateType)int.Parse(temp[offset].ToString());offset++;
		version = temp[offset].ToString();offset++;
	}
}

public class UpdateTable {
	public List<UpdateInfo> list;

	public UpdateTable(string content)
	{
		list = new List<UpdateInfo>();
		string[] rows = content.Trim().Replace("\r","").Split('\n');
		bool needHead = false;
		for(int i = 0;i<rows.Length;i++)
		{
			if(i == 0 && !needHead)
				continue;
			string[] cloums = rows[i].Split('\t');
			UpdateInfo info = new UpdateInfo(cloums);
			if(info != null)
			{
				list.Add(info);
			}
		}
	}
}
