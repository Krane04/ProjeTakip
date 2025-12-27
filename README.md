# Proje Yönetim ve Görev Takip Sistemi

Bu proje, Sakarya Üniversitesi Web Programlama dersi için geliştirilmiş; ekiplerin projelerini, görevlerini ve üyelerini yönetebileceği ASP.NET Core MVC tabanlı bir web uygulamasıdır.

##  Özellikler

Proje, ödev isterlerinin **Seçenek 3** (Proje ve Görev Takip Sistemi) başlığını kapsamakta olup aşağıdaki özellikleri içerir:

* **Rol Yönetimi:** Admin, Proje Yöneticisi (Manager) ve Ekip Üyesi (Member) rolleri.
* **Yetkilendirme:** * **Admin:** Tüm projeleri görür, üye rollerini değiştirebilir.
    * **Manager/Member:** Sadece dahil oldukları projeleri görebilirler (Kısıtlama).
* **Görev Takibi:** Görevler oluşturulabilir, kişilere atanabilir ve durumları (Beklemede, Yapılıyor, Tamamlandı) güncellenebilir.
* **Modern Arayüz:** Bootstrap 5 ve özel CSS ile responsive tasarım.

## Bonus Özellikler (Ekstra)

Standart isterlere ek olarak projeye şunlar eklenmiştir:
1.  **Admin Paneli:** Admin kullanıcıları, sistemdeki diğer üyelerin rollerini (Manager/Member) tek tıkla değiştirebilir.
2.  **Gelişmiş Üye Ekleme:** Projeye üye eklerken manuel e-posta yazmak yerine, sistemdeki kayıtlı kullanıcılar **Dropdown (Açılır Liste)** üzerinden seçilir.
3.  **Kullanıcı Bazlı Filtreleme:** Giriş yapan kullanıcı ana ekranda sadece kendi yetkili olduğu projeleri görür.
4.  **Task Repository Pattern:** Veri erişimi için Repository tasarım deseni (Pattern) kullanılmıştır.

##  Kurulum ve Çalıştırma Talimatları

Projeyi kendi bilgisayarınızda çalıştırmak için aşağıdaki adımları izleyin:

1.  **Veritabanı Oluşturma:**
    Visual Studio'da `Package Manager Console` penceresini açın ve şu komutu çalıştırın:
    ```bash
    update-database
    ```
    *Bu komut, `appsettings.json` dosyasındaki bağlantı dizesine göre yerel SQL Server (LocalDB)'da veritabanını ve tabloları otomatik oluşturacaktır.*

2.  **Projeyi Başlatma:**
    Projeyi `IIS Express` veya `Erkan_aktunc_web` profili ile başlatın (Ctrl + F5).

3.  **Giriş Bilgileri:**
    Proje ilk çalıştığında veritabanına otomatik olarak bir Admin kullanıcısı eklenir:
    * **Email:** `admin@sakarya.edu.tr`
    * **Şifre:** ``

## Kullanılan Teknolojiler

* ASP.NET Core 10.0 MVC
* Entity Framework Core (Code-First)
* ASP.NET Core Identity (Üyelik Sistemi)
* SQL Server
* Bootstrap 5 & Bootstrap Icons
