# Escena Invernal de Rescate (Unity)

Taller de laboratorio del módulo **Composición de Escenas y Control de Transformaciones**.

Es una noche de tormenta de nieve. Una patrulla de policía quedó atascada y medio sepultada bajo un ventisquero. El motor sigue encendido y escupe humo a tirones, las luces de emergencia giran sin parar y la ventisca arrecia sobre todo el entorno.

![Vista principal: patrulla enterrada en la nieve con sirenas, ventisca y humo de escape](Docs/captura_principal.png)

![Vista frontal de la patrulla](Docs/captura_frontal.png)

## Cómo abrir el proyecto

1. Instala [Git LFS](https://git-lfs.com) y ejecútalo una vez con `git lfs install`. Los modelos `.fbx` y las texturas `.png` se guardan con LFS: sin él, se descargan como punteros de texto y la escena aparece vacía.
2. Clona el repositorio.
3. Ábrelo con **Unity 6000.3.23f1**. El proyecto usa Universal Render Pipeline (URP).
4. Abre `Assets/Scenes/EscenaInvernal.unity` y pulsa **Play**: las sirenas giran, la nieve cae y el humo sale del escape.

## Jerarquía de la escena (10 objetos)

| # | Objeto | Tipo / componente | Función y posición |
|---|--------|-------------------|--------------------|
| 1 | `Main Camera` | Camera (+ Volume de post-procesado, antialiasing SMAA) | Encuadre diagonal trasero en (3.5, 1.55, -5.1). Muestra el coche, el humo, las huellas y las sirenas. |
| 2 | `Moon_Light` | Directional Light | Luz de luna azul pálido (0.72, 0.82, 1.0), intensidad 0.7, rasante (24°) para marcar el relieve de la nieve. |
| 3 | `Snow_Terrain` | Plano subdividido (FBX) + MeshCollider | Suelo nevado de 60 × 60 m con ventisqueros alrededor del coche, huellas de neumáticos y textura de nieve. (1) |
| 4 | `Police_Car` | Empty (objeto padre) | En **Y = -0.5**: el coche queda hundido y la nieve llega hasta las puertas. |
| 5 | ↳ `Car_Mesh` | Modelo 3D (FBX) | Carrocería de la patrulla con capas de nieve en techo, capó y maletero. Las ruedas quedan ocultas bajo la nieve. |
| 6 | ↳ `Siren_Pivot` | Empty + `SirenRotation.cs` | En el techo (0, 1.65, 0 local). Al girar, arrastra a sus dos luces hijas. |
| 7 | ↳↳ `Red_Light` | Spot Light (+ Lens Flare SRP) | Roja, rango 22, sobre el lente rojo de la barra y apuntando a un lado. |
| 8 | ↳↳ `Blue_Light` | Spot Light (+ Lens Flare SRP) | Azul, rango 22, sobre el lente azul y orientada al lado opuesto. |
| 9 | ↳ `Exhaust_Smoke` | Particle System | Humo del tubo de escape, en la parte trasera del coche. |
| 10 | `Global_Blizzard` | Particle System | Independiente: nieve cayendo sobre toda la escena. |

(1) **Sobre `Snow_Terrain`:** el taller pide un *Plane* o un *Terrain*. Aquí se usa un plano, pero modelado en Blender: una cuadrícula de 200 × 200 segmentos, en vez del *Plane* de Unity, que tiene solo 10 × 10. Así el suelo puede tener relieve real: ventisqueros más altos del lado del viento, montículos alrededor de la patrulla y huellas de neumáticos. Cumple la misma función de suelo que pide el taller y refuerza el efecto de "coche enterrado" que se evalúa en la composición.

## Script `SirenRotation.cs`

Está asignado **solo** a `Siren_Pivot`. La velocidad se ajusta desde el Inspector y el giro usa `Time.deltaTime`, así que no depende de los FPS.

```csharp
public class SirenRotation : MonoBehaviour
{
    public float rotationSpeed = 360f;   // grados por segundo

    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
```

Como `Red_Light` y `Blue_Light` son hijas del pivote, al rotarlo los dos haces de luz barren la nieve en círculo, como una sirena real. Cada luz tiene un componente *Lens Flare (SRP)*: cuando el haz apunta hacia la cámara, la barra destella con una pequeña estrella de su color.

## Sistemas de partículas

**`Global_Blizzard` (nieve)**

| Módulo | Configuración |
|--------|---------------|
| Shape | **Box** de 34 × 34 m a 12 m de altura, emitiendo hacia abajo |
| Main | Velocidad inicial variable (0.8 – 2.5), gravedad moderada (0.12), vida de 5 a 7 s, tamaño 0.035 – 0.09 |
| Emission | 8000 partículas/s, con prewarm para que nieve desde el primer frame |
| Velocity over Lifetime | Viento lateral constante (X entre -3 y -1.6) |
| Noise | Turbulencia suave (fuerza 0.6) |
| Color | Copos de tono azul claro que aparecen y desaparecen suavemente |
| Renderer | Stretched Billboard: los copos se estiran con el viento. Material *Particles/Simple Lit*: los copos que pasan cerca del coche se tiñen de rojo y azul con las sirenas |

**`Exhaust_Smoke` (escape del motor)**

| Módulo | Configuración |
|--------|---------------|
| Shape | **Cone** muy estrecho (6°, radio 0.04) apuntando hacia atrás y un poco hacia arriba |
| Emission | Curva que sube y baja, más una ráfaga (burst) en cada ciclo: el motor "tose" como si luchara contra el frío |
| Color over Lifetime | **Gris oscuro → transparente** |
| Size over Lifetime | **Tamaño creciente** (x0.5 → x3.4) |
| Otros | Leve flotación, el mismo viento que la nieve y rotación aleatoria de cada bocanada. *Soft particles*: el humo se desvanece al tocar la nieve en vez de cortarse en línea recta |

## Modelado y materiales

- El coche y el terreno se modelaron desde cero en **Blender** y se exportaron a FBX (`Assets/Models`).
  - La patrulla tiene esquema blanco y negro, ventanas con pilares, barra de luces, retrovisores, foco de búsqueda, antena, defensa delantera y el rótulo "POLICIA" atrás.
  - El terreno tiene ventisqueros más altos del lado del viento y huellas de neumáticos detrás del coche.
- Las texturas de la nieve (`Assets/Textures`) son procedurales y se repiten sin costuras: color, relieve de ondulaciones de viento, brillo con destellos de hielo y grano fino de detalle.
- Los materiales del coche y la nieve usan el shader URP Lit; las luces de la barra, los faros y las luces traseras son emisivos.
- La atmósfera es nocturna: niebla exponencial azul oscura, luz ambiental en degradado y reflejos del entorno atenuados.
- Post-procesado (`Assets/Settings/SampleSceneProfile.asset`): gradación de color fría (White Balance y Color Adjustments), Bloom para el brillo de las sirenas, Tonemapping y viñeta. La luna proyecta sombras suaves en calidad alta (shadow map de 4096).

## Estructura del proyecto

```
Assets/
├── Scenes/      EscenaInvernal.unity (la escena del taller)
├── Scripts/     SirenRotation.cs
├── Models/      Car_Mesh.fbx, Snow_Terrain.fbx
├── Materials/   coche, nieve y partículas
├── Textures/    texturas de nieve, copo y humo
└── Settings/    configuración de URP, post-procesado y destello de las sirenas
Docs/            capturas de este README
```

## Historial de commits

1. `feat: configuracion inicial del proyecto y escena vacia`
2. `feat: agregar terreno nevado y coche enterrado`
3. `feat: jerarquia de luces y sirenas de emergencia`
4. `feat: implementar script SirenRotation.cs`
5. `feat: integrar particulas de nieve y humo de escape`
6. `docs: README con descripcion de la escena y capturas`

## Herramientas

Unity 6000.3 (URP), Blender 5.2 y Git / GitHub con Git LFS. Como permite el taller, se usó un asistente de IA (Claude) como apoyo para los scripts y los parámetros de las partículas.
