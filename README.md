# Taller Unity: Escena Invernal de Rescate

Plan de trabajo basado en `Taller Práctico de Unity - Escena Invernal.pdf`.
Unity 6000.3.23f1 (URP). Tiempo estimado del taller: 2 a 3 horas.

**Escena:** una patrulla de policía queda semienterrada en una tormenta de nieve. El motor sigue encendido y emite humo. Las sirenas giran. La nieve cae sobre toda la escena.

## Estado inicial

| Elemento | Estado |
|---|---|
| Proyecto Unity | `SegundoTaller_EscenaInvernal/` con URP, `SampleScene` y `.gitignore` de Unity (activo) |
| Repo local | Tiene 1 commit ("Registro inicial") y cambios sin commitear |
| Remoto actual | `github.com/brandonjimenez21/SegundoTaller_EscenaInvernal` |
| Blender | `../SegundoTaller_EscenaInvernal.blend` (ver "Dependencias y bloqueos") |
| Unity MCP | Sin conexión (el editor está cerrado) |

## Dependencias y bloqueos

1. **Unity cerrado.** Abrir el proyecto en Unity 6000.3.23f1 para que el MCP de Unity conecte. Sin eso no se puede crear la escena ni capturar pantallas.
2. **Blender MCP.** En esta sesión no hay herramientas de Blender cargadas. Hay que abrir Blender con el addon de MCP activo y el servidor iniciado, o reconectar el MCP. Si no se logra, se usan primitivas de Unity (el PDF lo permite).
3. **Nombre del repo.** El PDF exige un repo **público** llamado `Unity-Taller-Invierno-[Apellido]`. El remoto actual tiene otro nombre. Decisión pendiente: crear el repo nuevo y cambiar el `origin`, o renombrar el actual.
4. **Cambios sin commitear.** Hay modificaciones de Unity (URP, ProjectSettings) y se borró `Assets/Editor/HubForceResolve.cs`. Revisarlas antes del commit 1 para no mezclarlas con otras features.

## Fases

### Fase 0. Preparación (sin código)
- [ ] Decidir el nombre del repo (ver bloqueo 3) y confirmar que es público.
- [ ] Abrir el `.blend` y ver qué contiene (coche, escala, origen, materiales).
- [ ] Abrir el proyecto en Unity y confirmar que el MCP conecta.
- [ ] Revisar `git status` y decidir qué entra en el commit 1.

### Fase 1. Proyecto base y escena vacía  →  Commit 1
- [ ] Verificar que `.gitignore` ignora `Library/`, `Temp/`, `Logs/`, `UserSettings/`.
- [ ] Guardar la escena como `Assets/Scenes/EscenaInvernal.unity` (o limpiar `SampleScene`) y dejarla vacía.
- [ ] Enlazar el `origin` al repo con el nombre exigido y hacer push.

Mensaje: `feat: configuracion inicial del proyecto y escena vacia`

### Fase 2. Terreno nevado y coche enterrado  →  Commit 2
- [ ] **Main Camera:** encuadre lateral/diagonal viendo el coche.
- [ ] **Snow_Terrain:** Plane o Terrain con material blanco o de nieve. Crear `Assets/Materials/Snow.mat`.
- [ ] **Police_Car:** Empty Object padre con **Y = -0.5 o menor**.
- [ ] **Car_Mesh:** hijo de `Police_Car`.
  - Opción A: modelo del `.blend` exportado a FBX en `Assets/Models/`.
  - Opción B: Cubes y Cápsulas (carrocería + cabina).
- [ ] Comprobar que las ruedas no se ven y que la nieve llega a las puertas. Ajustar Y hasta que se vea semienterrado.

Mensaje: `feat: agregar terreno nevado y coche enterrado`

### Fase 3. Luces y sirenas (sin código)  →  Commit 3
- [ ] **Moon_Light:** Directional Light, azul muy pálido, intensidad baja.
- [ ] **Siren_Pivot:** Empty hijo de `Police_Car`, ubicado en el techo.
- [ ] **Red_Light:** Point/Spot Light hijo de `Siren_Pivot`, rojo brillante, rango amplio.
- [ ] **Blue_Light:** Point/Spot Light hijo de `Siren_Pivot`, azul brillante, en el lado **opuesto** a la roja.

Mensaje: `feat: jerarquia de luces y sirenas de emergencia`

### Fase 4. Script de rotación  →  Commit 4
- [ ] Crear `Assets/Scripts/SirenRotation.cs`.
- [ ] Variable pública `rotationSpeed` (visible en el Inspector).
- [ ] En `Update()`: `transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);`
- [ ] Asignar el script **solo** a `Siren_Pivot`.
- [ ] Probar en Play: las luces giran en círculo, sin errores en la consola.

Mensaje: `feat: implementar script SirenRotation.cs`

### Fase 5. Partículas  →  Commit 5
- [ ] **Global_Blizzard** (independiente, no es hijo de nada):
  - Shape: Box o Plane, alto sobre la escena, emitiendo hacia abajo.
  - Gravedad moderada, velocidad inicial variable (Random Between Two Constants).
  - Material de partícula por defecto o textura suave, con ligero tono azul claro.
- [ ] **Exhaust_Smoke** (hijo de `Police_Car`, en la parte trasera):
  - Shape: Cone muy estrecho.
  - Color over Lifetime: gris oscuro a transparente.
  - Size over Lifetime: creciente.
  - Emisión baja o irregular para sugerir que el motor lucha por mantenerse encendido.

Mensaje: `feat: integrar particulas de nieve y humo de escape`

### Fase 6. Verificación final y entrega
- [ ] La jerarquía tiene **exactamente 10 objetos** con los nombres y padres de la tabla de abajo.
- [ ] Play sin errores en la consola. Capturas de la escena (Game view).
- [ ] `git log --oneline` muestra al menos 5 commits con el formato pedido.
- [ ] Repo público y sin `Library/` subida. Entregar el enlace de GitHub.

## Jerarquía objetivo (exactamente 10 objetos)

```
Main Camera
Moon_Light
Snow_Terrain
Police_Car              (Empty, Y <= -0.5)
├── Car_Mesh
├── Siren_Pivot         (script SirenRotation)
│   ├── Red_Light
│   └── Blue_Light
└── Exhaust_Smoke       (Particle System)
Global_Blizzard         (Particle System, independiente)
```

Cuidado: la rúbrica exige exactamente estos 10. No dejar objetos extra en la escena, como el `Directional Light` por defecto o un `Global Volume` que traiga la plantilla.

## Rúbrica

| Criterio | Peso | Qué lo asegura |
|---|---|---|
| Jerarquía y composición | 30% | Fases 2, 3 y verificación de los 10 objetos; coche semienterrado |
| Control de versiones | 30% | 5 commits atómicos con el formato exacto del PDF |
| Scripting | 20% | `Time.deltaTime`, `rotationSpeed` pública, script solo en `Siren_Pivot` |
| Partículas | 20% | Fase 5, ambos sistemas visibles en Play |

## Flujo de commits

Un commit por fase, en orden, sin mezclar features. Los comandos de commit y push los ejecuta el dueño del repo. Al terminar cada fase se dan los comandos listos para copiar:

```
git add -A
git commit -m "feat: ..."
```

## Decisiones abiertas

1. ¿Repo nuevo con el nombre `Unity-Taller-Invierno-[Apellido]`, o renombrar el actual? ¿Cuál es el apellido a usar?
2. ¿El coche sale del `.blend` (FBX) o se arma con primitivas de Unity?
3. ¿Se conserva el commit "Registro inicial" como base, o el commit 1 debe ser el de configuración?
