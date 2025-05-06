# PaparaGo WebAPI

Papara Kadın Yazılımcı Bootcamp bitirme projesi kapsamında geliştirilen bu Web API, sahada çalışan personelin masraf girişlerini yapabilmesini ve şirket yöneticisinin bu talepleri onaylayıp ödeme işlemlerini gerçekleştirmesini sağlayan bir sistemdir.

## 🚀 Proje Hakkında

Bu uygulama ile:

- Personel, yaptığı harcamaları anında sisteme girebilir.
- Yöneticiler talepleri görüntüleyip onaylayabilir veya reddedebilir.
- Onaylanan talepler için ödeme işlemleri sanal olarak simüle edilir.
- Kullanıcı rolleri: Admin ve Personel.
- Masraf talepleri kategorilere ayrılmıştır ve evrak yüklenmesi desteklenmektedir.

## 🔧 Teknolojiler

- .NET Core Web API
- JWT Authentication
- Swagger (API dokümantasyonu)
- PostgreSQL 
- Postman Collection

## 📁 Proje Kurulumu

### 1. Veritabanı

[🔗NEON](https://neon.tech/) PostgreSQL

## 🧪 Test / Dokümantasyon
- Swagger üzerinde tüm uçtan uca senaryolar test edilebilir.
- API dokümantasyonu ayrıca Postman Collection olarak dışa aktarılmıştır.

## 🧩 API Özellikleri

### Yetkilendirme
- JWT Token yapısı ile login olduktan sonra erişim sağlanır.
- /api/Auth/login → Token alınır.

### Personel İşlemleri
- Masraf oluşturma
- Taleplerini listeleme (aktif/geçmiş)
- Red sebebini görüntüleme
- Raporlama (günlük, haftalık, aylık)

### Admin İşlemleri
- Tüm talepleri onaylama/red etme
- Kategori yönetimi (CRUD)
- Personel yönetimi (CRUD)
- Raporlama

### Raporlama
- Toplam masraf özetleri (günlük/haftalık/aylık)
- Personel bazlı masraf yoğunluğu
- Kategori dağılımları
- Raporlamalarda Dapper ve view/SP kullanılmaktadır.

## 🔒 Validasyonlar
- Her endpoint için gerekli model validasyonları yapılmıştır.
- Boş bırakılamayacak alanlar kontrol edilmiştir.
- Ödeme tutarı sıfırdan küçük olamaz.
- Silinmek istenen kategoride aktif talep varsa silme işlemi engellenir.
