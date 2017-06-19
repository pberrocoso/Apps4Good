# Aplicación concurso Apps4Good de Microsoft#

El proyecto se denomina: **Share the Road**

Desde hace ya varios años, observamos en los medios de comunicación los accidentes de tráfico en los que desgraciadamente se ven ciclistas involucrados .

La función principal de la aplicación es situar elementos de riesgo (Ciclistas principalmente, pero también podrían ser peatones, vehículos de velocidad reducida) en la ruta de los conductores, para de esta forma prevenir y advertir del posible riesgo que se van a encontrar unos metros más adelante.

La aplicación está diseñada para adoptar tanto el rol del ciclista, enviando su posición a un backend de Azure dónde se almacena en una base de datos SQL Server, como del conductor, leyendo del mencionado backend y posicionando en un mapa los elementos de riesgo situados a una distancia determinada. Adicionalmente, mediante textToSpeech se anuncia por locución dichos elementos.

## Descripción de los proyectos ##

### Backend de Azure ##

El backend de Azure es una Azure MobileApp que aloja un WebAPI (El proyecto destá dentro de la carpeta AzureBackend). El WebAPI expone un servicio REST con dos operaciones, tal como se expone en la siguiente descripción con SWAGGER:

[http://sidermobile.azurewebsites.net/Content/def/index.html](http://sidermobile.azurewebsites.net/Content/def/index.html)

La operación "Position" inserta en una BBDD SQL Server las posiciones de los ciclistas.
La operación "Cercanos" obtiene los posibles ciclistas dada unas coordenadas.

La solución cuenta con un proyecto que expone el WebAPI, una librería de clases para desacoplar la conexión contra la BBDD y un proyecto de Test.

### Aplicación móvil ###

En la carpeta "MobileApp/Sidercar" está la solución crossplatform de Xamarin. Incluye la librería portable (PCL) donde se lleva a cabo la conexión con los servicios y dos vistas que son comunes al resto de plataformas: La pantalla "Home" y la pantalla de "Configuración". El resto de pantallas he decidido hacerlas nativas **(de momento sólo en Android)** por necesitar de un servicio de Backgroud donde continuar con la ejecución aunque la aplicación se vaya a segundo plano.

**Pantalla Home inicial de la aplicación (XAML)**

![](https://image.ibb.co/f4FfXk/Pantalla_Home.png)

Esta es la pantalla principal de la aplicación. Está en la PCL con XAML y los dos primeros botones llaman  mediante la "Dependencia de servicio" a las vistas nativas en cada plataforma.

**Pantalla de envío de datos del ciclista**

![](https://image.ibb.co/nzN5yQ/Pantalla_Ciclista.png)

Esta pantalla envía los datos al WebAPI de azure y se grabarán en BBDD. Se usa vista nativa para usar el backgroud service de cada plataforma.

**Pantalla del conductor**

![](https://image.ibb.co/mobXdQ/Pantalla_Vehiculo.png)

El conductor se conecta al WebAPI para enviar su posición y consultar si hay ciclistas cerca en su ruta. La ruta se obtiene **conectando con los servicios de google maps** que convierte coordenadas en direcciones. Si existen ciclistas se pintan en el mapa y se realiza una locución de voz advirtiendo del riesgo.

**Pantalla de configuración**

![](https://image.ibb.co/kAo0Xk/Pantalla_Settings.png)

La pantalla de configuración permite modificar los valores de "Tiempo": el tiempo que debe pasar desde que el ciclista insertó la posición para que la tenga en cuenta. No queremos que nos dé la posición de un ciclista que estuvo ahí hace media hora. Y también los de "Distancia": la distancia mínima a la que quieres que te advierta. Un tractor puede necesitar 500 metros y no un kilómetro para poder reaccionar.

Los datos de la configuración se almacenan con sqlLite en el teléfono del usuario.

**Básicamente esta es la descripción de la aplicación. Creo que cumple una buena labor "social" para tratar de impedir accidentes de tráfico con ciclistas o cualquier otro elemento de riesgo que podamos encontrarnos en la carretera.**

**Espero que guste y que se valore la arquitectura distribuída que se ha utilizado para conectar la aplicación con los servicios de Azure. **

**Para cualquier aclaración, por favor no dudéis en poneros en contacto conmigo a través de mi email: pedro.berrocoso@gmail.com**

**GRACIAS!!!**
