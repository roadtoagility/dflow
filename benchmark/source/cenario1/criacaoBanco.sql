--CREATE DATABASE BD_BENCHMARK

use BD_BENCHMARK

CREATE TABLE TB_PRODUCT(
    Id int IDENTITY(1,1) PRIMARY KEY,
    Name varchar(200),
    Description varchar(600),
    Price decimal(18,6),
)


insert into TB_PRODUCT values('Kit Cerveja Brahma',
    'Kit Cerveja Brahma Chopp Pilsen 269ml Cada 15 Unidades com 2 Copos', 34.90)

insert into TB_PRODUCT values('Smart TV Full HD LED 43',
                              'Smart TV Full HD LED 43” Philco PTV43G50SN Android Wi-Fi 3 HDMI 2 USB', 1196.05)

insert into TB_PRODUCT values('Boleira de Vidro Redonda',
                              'Boleira de Vidro Redonda com Tampa de Acrílico Ruvolo Bari 32cm', 39.90)

insert into TB_PRODUCT values('Smartphone Motorola E6 Plus 64GB',
                              'Smartphone Motorola E6 Plus 64GB Rubi 4G 4GB RAM Tela 6,1” Câm. Dupla + Câm. Selfie 8MP Dual Chip', 743.07)

insert into TB_PRODUCT values('Kit Cerveja Budweiser American Standard',
                              'Kit Cerveja Budweiser American Standard Lager 269ml Cada 8 Unidades com 1 Copo', 29.90)

insert into TB_PRODUCT values('Lenço Umedecido MamyPoko',
                              'Lenço Umedecido MamyPoko Toque Suave 200 Unidades', 26.90)

insert into TB_PRODUCT values('Smart TV LED 32” Philco',
                              'Smart TV LED 32” Philco PTV32G60SNBL Wi-Fi 2 HDMI 1 USB', 899.65)

insert into TB_PRODUCT values('Pilha AA Alcalina 16 Unidades Duracell',
                              'Pilha AA Alcalina 16 Unidades Duracell', 34.90)

insert into TB_PRODUCT values('Smartphone Motorola E6 Plus 32GB',
                              'Smartphone Motorola E6 Plus 32GB Rubi 4G 2GB RAM Tela 6,1” Câm. Dupla + Câm. Selfie 8MP Dual Chip', 650.07)

insert into TB_PRODUCT values('Jogo de Panelas Tramontina Antiaderente',
                              'Jogo de Panelas Tramontina Antiaderente de Alumínio Vermelho 10 Peças Turim 20298/722', 269.90)

select * from TB_PRODUCT


