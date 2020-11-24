public class Slot : IComparable
{
    public int CompareTo(object obj) { return 0; }

    public int SlotID { get; set; }

    public int ClothesID { get; set; }

    public string ClothesName { get; set; }

    public string SizeName { get; set; }
}

object slot = new Slot() { ClothesName = "上衣" };
{
    if (slot is Slot)
    {
        $"slot is {nameof(Slot)}".Dump();
    }

    var query = (Slot)slot;
    $"slot is {nameof(Slot)},CLothesName={query.ClothesName}".Dump();

    var query2 = slot as Slot;
    if (query2 != null)
    {
        $"slot is {nameof(Slot)},ClothesName={query2.ClothesName}".Dump();
    }

    if (slot is Slot query3)
    {
        $"slot is {nameof(Slot)},ClothesName={query3.ClothesName}".Dump();
    }
}
{
    object e = 150;
    if (e is null) "e is null".Dump();
    if (e is not null) "e is not null".Dump();
    if (e is 150) $"e is {e}".Dump();

    if (e is >= 100 and <= 200) $"e = {e}, 在 >=100 and <=200".Dump();
    if (e is int i and >= 100 and <= 200) $"e = {i}, 在 >=100 and <=200".Dump();
    if (e is 100 or 150 or 200) $"e = {e}, 在 is 100 or 150 or 200".Dump();
    if (e is not null and not "") $"e = {e}, 在 is not null and not ".Dump();
}
(int, int) tp = (1, 2);
if (tp is (1, 2)) "is can check tuple".Dump();
{
    //is 和 var的结合
    int f = 150;
    if (f is var i && i >= 100 && i <= 200) $"f = {i}, 在 >=100 and <=200".Dump();
}

{
  var slotList=new List<Slot>(){
    new Slot() {SlotID=1, ClothesID=10,ClothesName="上衣",SizeName="L"},
    new Slot() {SlotID=1, ClothesID=20,ClothesName="裤子",SizeName="M"},
    new Slot() {SlotID=1, ClothesID=11,ClothesName="皮带",SizeName="X"},
    new Slot() {SlotID=2, ClothesID=30,ClothesName="上衣",SizeName="L"},
    new Slot() {SlotID=2, ClothesID=40,ClothesName="裤子",SizeName="L"},
  };
  slotList.Select(p=>p.ClothesID).Dump();
   //找到 刚好挂了一件裤子L & 一件上衣L  & 总衣服个数=2  的 挂孔号
   var query = slotList.GroupBy(m=>m.SlotID).Where(m=>m.Where(n=>n.SizeName=="L").ToList()
   is var clothesList && clothesList.Count(k=>k.ClothesName == "裤子") is 1 &&
   clothesList.Count(k=>k.ClothesName == "上衣") is 1 && m.Key==2).ToDictionary(k=>k.Key,v=>v.ToList());
   query.SelectMany(p=>p.Value.Select(s=>s.ClothesID)).Dump();
}
