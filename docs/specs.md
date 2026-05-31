# Specs - Companion JDR

> Derniere mise a jour : 2026-05-28

## 1. Objectif et contexte

Construire une application compagnon JDR orientee gestion long terme de campagne.

Le produit ne remplace pas les outils de plateau virtuel (Roll20, MapTool). Il est utilise en parallele pour centraliser :
- le suivi des fiches de personnage,
- la base de connaissances regles (sorts, competences, armes, equipements),
- le partage de donnees entre MJ et joueurs,
- la persistance locale et la synchronisation multi-appareils.

## 2. Vision produit

- Positionnement : "second cerveau" de campagne.
- Systeme cible initial : DnD 3.5.
- Contrainte strategique : architecture suffisamment maleable pour integrer d autres sets de regles sans refonte majeure.
- Distribution : mobile + desktop via client unique (Flutter prioritaire, Web comme alternative selon faisabilite).

## 3. Perimetre PMV (et hors-scope explicite)

### 3.1 PMV

Le PMV doit livrer les fonctionnalites fondamentales gratuites :

1. Gestion de compte local et profils joueur/MJ.
2. Creation et edition de fiches personnages.
3. Bibliotheque locale de reference : sorts, competences, armes.
4. Recherche, filtres, favoris, liens entre elements de reference.
5. Sauvegarde locale SQLite offline-first.
6. Synchronisation vers API centrale pour backup et partage basique de fiches.
7. Partage de fiche a des utilisateurs d une meme session/campagne.
8. Import de donnees de reference "BDD maitre" en lecture.

### 3.2 Hors-scope PMV

1. Remplacement de VTT (carte, fog of war, initiative temps reel avancee).
2. Automatisation complete des regles de combat.
3. Integrations profondes natives avec APIs Roll20/MapTool.
4. Marketplace de contenus communautaires.
5. Moteur de scripts regles avances multi-systemes.

## 4. Utilisateurs et droits

### 4.1 Roles

1. Joueur :
- gere ses fiches,
- consulte la base de reference,
- partage ses fiches avec son MJ,
- consomme des fonctions premium si abonnement actif.

2. MJ :
- consulte les fiches partagees,
- gere la session/campagne,
- peut pousser des references/notes aux joueurs,
- peut auto-heberger ses donnees personnelles (priorite roadmap).

3. Admin plateforme (back-office) :
- maintient la BDD maitre,
- publie des mises a jour de donnees,
- gere abonnements et entitlements premium.

### 4.2 Regles de partage minimales

- Une fiche appartient a un proprietaire.
- Le proprietaire peut accorder des droits de lecture (et optionnellement edition) a d autres utilisateurs.
- Le MJ peut recevoir un acces lecture aux fiches des joueurs de sa campagne.

## 5. Cinematique principale (happy path)

1. L utilisateur installe l app et cree un compte local.
2. Il cree ou importe une fiche personnage.
3. Il navigue dans la base sorts/competences/armes et attache des elements a sa fiche.
4. L app sauvegarde toutes les modifications en local (SQLite).
5. Si connexion disponible, l app synchronise les changements vers l API centrale.
6. Le joueur partage sa fiche avec le MJ via une invitation/session.
7. Le MJ consulte la fiche partagee dans sa propre app.

## 6. Gestion des erreurs et cas limites

1. Offline total : fonctionnement local complet des fonctions PMV, sync differree.
2. Conflits de synchronisation :
- strategie PMV "last write wins" par champ + journal de modifications,
- ecran de resolution manuelle pour champs critiques (nom, niveau, stats).
3. API indisponible : file d attente locale et reprise automatique.
4. Perte d appareil : restauration via derniere sync cloud ou export local.
5. Donnee de reference mise a jour : versionning de dataset pour compatibilite.

## 7. Modele de donnees (niveau produit)

### 7.1 Entites principales

- User
- Campaign
- Session
- Character
- CharacterShare
- Spell
- Skill
- Weapon
- RuleSet
- RuleEntityMapping
- Subscription
- Entitlement
- SyncEvent

### 7.2 Principes de modelisation

1. Separation stricte entre :
- donnees metier utilisateur (fiches, campagnes),
- donnees de reference (BDD maitre),
- moteur de regles (metadonnees de calcul/validation).

2. Extensibilite multi-systemes :
- chaque element de reference est tagge par RuleSet (ex: dnd-3.5),
- les champs specifiques systeme sont portes via schema extensible (JSON typage controle).

3. Local-first :
- chaque enregistrement est addressable localement (UUID),
- metadonnees de sync (updatedAt, version, tombstone) sur toutes les entites synchronisees.

## 8. Architecture technique cible

## 8.1 Client

Option recommandee : Flutter (mobile + desktop) avec architecture modulaire.

- Persistence locale : SQLite.
- Couche acces donnees : repository + cache + sync queue.
- Moteur de regles : plugin interne par RuleSet.
- UI : ecrans fiche, bibliotheque reference, partage, conflits de sync.

## 8.2 API centrale

Backend C#/.NET centralise :
- authentification,
- sync de donnees utilisateur,
- partage de fiches,
- distribution des datasets de reference,
- gestion abonnement/entitlements.

## 8.3 Self-host (minimum viable)

Objectif : self-host des donnees personnelles au minimum.

Perimetre initial self-host :
- stockage des fiches utilisateur,
- sync basique,
- partage local reseau prive (optionnel phase 2).

Approche recommandee :
- fournir une image conteneur backend,
- config simple via variables d environnement,
- mode "reference cloud + user data self-host" possible.

## 9. Integrations externes

1. VTT tiers (Roll20/MapTool) :
- pas d integration profonde PMV,
- approche "utilisation parallele" + export/import leger (phase ulterieure).

2. Paiement abonnement :
- Patreon envisage,
- necessite une couche entitlement dediee cote backend pour activer features premium de maniere fiable.

## 10. Montee en gamme (Freemium -> Premium)

### 10.1 Gratuit

- Fiches personnages,
- consultation base de reference,
- sync basique,
- partage de base MJ/joueur.

### 10.2 Premium

- scaling automatise des stats selon caracteristiques,
- dice roller integre,
- gestion avancee des sorts prepares,
- automatisations contextuelles de regles,
- (potentiel) modules systeme avances.

## 11. Contraintes techniques et conventions

1. Offline-first obligatoire.
2. API idempotente pour sync robuste.
3. Versionning explicite des RuleSets et datasets de reference.
4. Journalisation des operations de sync pour debug.
5. Chiffrement des tokens et donnees sensibles sur client.

## 12. Roadmap proposee

### Phase 1 - Fondations

1. Schema local SQLite + modele Character/Spell/Skill/Weapon.
2. CRUD fiche personnage.
3. Import dataset DnD 3.5 reference.
4. API .NET minimale (auth + sync fiches).

### Phase 2 - Collaboration

1. Partage MJ/joueur.
2. Gestion de campagne/session.
3. Resolution de conflits sync.

### Phase 3 - Premium

1. Entitlements abonnement.
2. Scaling stats automatise.
3. Dice roller.
4. Gestion sorts prepares.

### Phase 4 - Ouverture

1. Self-host pack simplifie.
2. Framework RuleSet pour autres jeux.
3. Connecteurs externes legerement couples (import/export).

## 13. KPI produit

1. Temps de creation fiche < 5 min.
2. Taux d erreurs de sync < 1% des operations.
3. Retention campagne a 30 jours.
4. Taux d activation partage MJ/joueur.
5. Conversion premium sur fonctionnalites avancees.

## 14. Zones d ombre et decisions a prendre

1. Flutter vs Web prioritaire :
- Flutter conseille pour convergence mobile/desktop,
- Web peut rester client secondaire ou console admin.

2. Strategie exacte de monetisation :
- Patreon seul,
- ou abonnement natif avec gestion complete dans la plateforme.

3. Niveau de complexite du moteur multi-regles :
- metadata-driven simple,
- ou mini moteur de regles declaratif.

4. Limites self-host :
- seulement stockage perso,
- ou federation avec partage multi-utilisateurs.

5. Gouvernance de la BDD maitre :
- pipeline de mise a jour,
- controle qualite des donnees,
- politique de versionnage.
