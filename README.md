# Game Project

## Installation rapide

### 1. Cloner le repository

```bash
git clone [URL_DU_REPO]
cd gameproject
```

### 2. Télécharger les assets

Les assets statiques (modèles 3D, textures, audio) ne sont pas inclus dans le repository pour garder le repo léger (~15 MB).

**Téléchargez les 3 parties des assets depuis les releases GitHub :**

1. **RELEASE_ASSETS_PART1.7z** (~571 MB)
2. **RELEASE_ASSETS_PART2.7z** (~610 MB)
3. **RELEASE_ASSETS_PART3.7z** (~461 MB)

### 3. Installer les assets dans le repo

1. Décompressez les 3 archives
2. Pour chaque partie, copiez le contenu du dossier `Assets/` dans votre projet Unity :
   ```
   RELEASE_ASSETS_PART1/Assets/* → VotreProjet/Assets/
   RELEASE_ASSETS_PART2/Assets/* → VotreProjet/Assets/
   RELEASE_ASSETS_PART3/Assets/* → VotreProjet/Assets/
   ```
3. Ouvrez Unity - les assets seront automatiquement importés

### 4. Structure finale

Après l'installation, votre projet devrait avoir cette structure :
```
Assets/
├── FpsHorrorKit/          (assets statiques - du release package)
│   ├── Models/
│   ├── Textures/
│   ├── Audio/
│   └── ...
├── Scripts/                (code source - dans le repo)
├── Prefabs/                (prefabs - dans le repo)
├── Scenes/                 (scènes - dans le repo)
└── ...
```

## Notes

- Le repository contient uniquement les fichiers dynamiques (code, prefabs, scènes) - ~15 MB
- Les assets statiques (~1.6 GB) sont dans le release package
- Si vous modifiez des assets statiques, vous devrez les réexporter dans le release package
- Lisez vraiment bien les Issues, pour savoir comment utiliser git, ou si vous comprenez pas regardez un tuto, il est tres important de utiliser parceque c'est un grand project, et je parle pas de la technologie dedans mais je parle de enorme contenu des fichiers qui fais totalement chier.
