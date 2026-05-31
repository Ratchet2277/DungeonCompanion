# ADR-001: Flutter comme framework client cross-platform plutôt que .NET MAUI

**Date:** 2026-05-28  
**Status:** ACCEPTED  
**Context:** Projet compagnon JDR, offline-first, cible mobile + desktop + web  

## Contexte

Le projet doit livrer une application client capable de:
1. Fonctionner offline en local (SQLite).
2. Synchroniser via API .NET centrale.
3. Couvrir Android, iOS, Windows, macOS, et Web.
4. Offrir une UX fluide et native-like, surtout sur mobile.

Deux options principales ont été évaluées:
1. **Flutter**: Dart, un seul codebase cross-platform, Web possible.
2. **.NET MAUI**: C#, partage potentiel de code avec backend .NET, pas de support Linux officiel.

## Décision

**Nous choisissons Flutter comme framework client principal.**

Raison dominante: **meilleure UX mobile native + temps de mise en marché + écosystème offline-first.**

## Justification

### Avantages Flutter
1. **UX mobile native-like** (iOS et Android) immédiatement.
2. **Écosystème offline** très mature (Drift/SQLite, Riverpod, caching patterns bien établis).
3. **Web en bonus**: Flutter Web compasse vers le navigateur avec même codebase, acceptable pour consultation/admin.
4. **Pas de WebView obligatoire** (contrairement à Blazor Hybrid qui impose WebView).
5. **Génération client Dart** depuis OpenAPI automatisée (swagger_dart_code_generator).
6. **Support de Linux** en cas de besoin futur.
7. **Communauté large** et patterns de sync/offline bien documentés.

### Avantages .NET MAUI
1. Partage de code DTO avec backend (réduction mapping théorique).
2. Un seul langage pour toute l'équipe.

### Trade-offs acceptés
1. **Mapping explicite**: On accepte du mapping DTO -> Domain -> Local car:
   - Le mapping est nécessaire quand même en offline (champs sync-specific).
   - L'abstraction est pédagogiquement utile (apprentissage architecture).
   - OpenAPI génération automatise la plupart du DTO -> Domain.

2. **Écosystème C# réduit côté client**: 
   - On peut ajouter un back-office séparé en .NET (console admin, gestion BDD maitre) si besoin.
   - Cela ne force pas le client principal en C#.

3. **Moins de partage de code**:
   - Les interfaces/contrats partagés sont limités (au contraire du full-stack C#).
   - Acceptable: séparation client/serveur est architecturalement plus propre long-terme.

## Conséquences

### Positives
1. UX optimisée mobile dès V1 → meilleure adoption utilisateurs.
2. Web en bonus sans refonte majeure.
3. Offline robuste avec patterns éprouvés (Drift, cache layers).
4. Spike technique + apprentissage clair (architecture offline-first = educational value).

### Négatives
1. Équipe doit connaître Dart (learning curve si background backend C#).
2. Moins de partage code client/serveur (mapping à entretenir).
3. Debug cross-platform plus exigeant qu'une stack unique.

### Mitigation
1. Formation Dart/Flutter rapide (syntaxe proche TypeScript/C#).
2. Couche data abstraite stricte (Repository pattern) facilite maintenance.
3. Tests unitaires sur mappings (évite régressions).

## Alternatives considérées et rejetées

### 1. Full stack C# (.NET MAUI + backend .NET)
- **Rejeté**: partage code théorique ne compense pas UX mobile moins bonne, WebView obligatoire en MAUI Blazor, pas de support native-like aussi bon.

### 2. Web-first (TypeScript React + Tauri pour desktop)
- **Rejeté**: UX mobile dégradée sur PWA, offline compliqué en IndexedDB, focus produit s'éparpille entre web et desktop.

### 3. Multi-client (React Web + Flutter mobile)
- **Rejeté**: double maintenance, divergences inévitables, coût d'entrée plus haut.

## Validations requises

1. Spike Drift + SQLite sur mobile + web (vérifier abstraction data).
2. Prototype sync offline + conflits (vérifier approche resilience).
3. OpenAPI generation client Dart (vérifier time-saving vs manual mapping).

## Références

- Flutter docs: https://flutter.dev/docs
- Drift (SQLite ORM pour Dart): https://drift.simonbinder.eu/
- swagger_dart_code_generator: https://pub.dev/packages/swagger_dart_code_generator
