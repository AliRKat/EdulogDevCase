README for Edulog Developer Case

Projedeki Modüller:
- Player: Kontrol ettiğimiz karakterle alakalı temel çoğu şeyi kontrol eden Singleton class.
  - PlayerAnimation: Karakterin animasyon geçişlerini gerçekleştirir.
  - PlayerEquipment: Karakterin kullandığı araç gereçlerin kontrolünü gerçekleştirir.
  - PlayerGathering: Karakterin gather edebildiği objelerle etkileşimini sağlayan ve gather etmesini mümkün kılar.
  - PlayerInventory: Karakterin topladığı eşyaları taşıdığı, parası, taşıma kapasitesi gibi detayları yönetir.
  - PlayerLevel: Karakterin XP progression'unu takip eden ve alakalı ilerlemeyi gerçekleştirir.
  - PlayerMovement: Karakterin hareketini NavMeshAgent aracılığı ile tıklanılan noktaya gidilmesi şeklinde gerçekleştirir.

- Manager Classlar:
  - GameManager: Oyunun genel işleyişini kontrol eden Singleton class.
  - QuestManager: Görev verilmesi ve tamamlanan görevlerden ödül verilmesi ile sorumldur.
  - SoundManager: Ana menüde settings başlığındaki sound seçeneği açık ise arka fondaki ses efektini oynatır.
  - MarketUIManager: Market'e gidildiğindeki UI'ın buy ve sell ekranlarını düzenler.
  - MainMenuManager: Ana menüyü yönetir.
  - EquipmentUIManager: I tuşuna basıldığında açılan envanter menüsünü yönetir.

- Önemli Diğer Yapılar:
  - Quest: Mümkün olduğunca geniş bir şekilde tasarlanmış genel görev objesi. Her quest kendine ait unique bir ID'ye sahiptir.
  - Equipment: UpgradeableSO adındaki scriptable object yapısını kullanan, Player'ın kullandığı eşyaların temel yapısı.
  - ItemBase: Player'ın gather edebildiği objelerin temel yapısı
  - SelectableObject: Player'ın çevre ile interaksiyona girebilmesini sağlayan ve Highlight etmesini sağlayan yapı.

Bunlar dışında proje içerisinde aynı zamanda Editor pencereleri de yazılmıştır, bu pencerelerin temel amacı oyun içerisinde bug yaratmak, progression hızlandırmak ve test gerçekleştirebilmektir.
  - InventoryManagerEditor: Runtime içinde Inventory'e herhangi bir isimdeki bir objeyi girilen miktar ve value değerleri ile ekler
  - PlayerXPManager: Runtime içerisinde player'a girilen miktarda XP yükleyerek bir anda dilendiği kadar level atlanmasını sağlar.
  - SaveManagerEditor: Oyunun kullandığı save yapılarını teker teker veya toplu olarak silmeye imkan sağlar.

Eksik veya Tamamlanmamış Bölümler:
Şu anda proje içerisinde talep edilen temel "Menü, görev sistemi, hasat yönetimi, envanter ve market" yapıları mekaniksel olarak sorunsuz bir şekilde bulunmaktadır.
Hasat yönetiminde görsel bir eksiklik olarak Player'a bir animasyon verildi. Aynı zamanda daha fazla görsel efekt kullanımı gerçekleştirilebilir.
Görev sisteminde şu anda temel bir görev yapısı olarak sonsuza kadar sürecek şekilde Player'ın eşyalarını upgrade etme görevi gelmekte. Buna yan görevler eklenebilir veya görev yapısı kolaylıkla çoğaltılabilir.

Test Talimatları:
Karakteri hareket ettirmek için Mouse kullanın. Sol click gerçekleştirlen yere karakter otomatik olarak hareket edecektir. Başladığınız konumun solunda ve sağında tarla, ön tarafınızda başlangıç objesi olarak kürek ve yolun ilerisinde market bulunmakta.
Herhangi bir obje ile etkileşime girmek için mouse üzerinde hover ettiğinde, obje highlight olmuşken tıklanması yeterli olacaktır.
Kürek alındıktan sonra I tuşuna basarak envanter penceresi açılabilir, burada Equip, Unequip, Drop gibi seçenekler bulunmaktadır.
Esc tuşuna basıldığı takdirde açılan menüde ana menüye dönülebilir veya oyun kapatılabilir.
Arka fonda loop halinde dönen ses efekti, ana menüde settings kısmından toggle uncheck edilerek kapatılabilir.

Bilinen Hatalar:
SaveManager'da belli açıklar olmasından ötürü envanterdeki eşyalar oyuna gir çık yapıldığında kaybolabiliyor. 
