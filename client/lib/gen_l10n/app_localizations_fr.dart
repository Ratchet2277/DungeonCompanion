// ignore: unused_import
import 'package:intl/intl.dart' as intl;
import 'app_localizations.dart';

// ignore_for_file: type=lint

/// The translations for French (`fr`).
class AppLocalizationsFr extends AppLocalizations {
  AppLocalizationsFr([String locale = 'fr']) : super(locale);

  @override
  String get appTitle => 'Compagnon Donjon';

  @override
  String get appSubtitle => 'Gérez vos personnages D&D';

  @override
  String get navigationHome => 'Accueil';

  @override
  String get navigationCharacters => 'Personnages';

  @override
  String get navigationSpells => 'Sorts';

  @override
  String get navigationSkills => 'Compétences';

  @override
  String get navigationSettings => 'Paramètres';

  @override
  String get charactersTitle => 'Mes Personnages';

  @override
  String get charactersEmpty =>
      'Aucun personnage créé. Créez votre premier personnage !';

  @override
  String get charactersCreateButton => 'Nouveau Personnage';

  @override
  String get characterLevel => 'Niveau';

  @override
  String get characterExperience => 'Expérience';

  @override
  String get characterHealth => 'Points de vie';

  @override
  String get characterAttributes => 'Attributs';

  @override
  String get characterSpells => 'Sorts';

  @override
  String get characterSkills => 'Compétences';

  @override
  String get spellsTitle => 'Sorts';

  @override
  String get spellsEmpty => 'Aucun sort disponible';

  @override
  String get spellLevel => 'Niveau';

  @override
  String get spellSchool => 'École';

  @override
  String get spellCastingTime => 'Temps d\'incantation';

  @override
  String get spellRange => 'Portée';

  @override
  String get spellComponent => 'Composante';

  @override
  String get spellDuration => 'Durée';

  @override
  String get spellSavingThrow => 'Jet de sauvegarde';

  @override
  String get spellDescription => 'Description';

  @override
  String get skillsTitle => 'Compétences';

  @override
  String get skillsEmpty => 'Aucune compétence disponible';

  @override
  String get skillKeyAbility => 'Capacité clé';

  @override
  String get skillTrainedOnly => 'Entraîné uniquement';

  @override
  String get skillArmorCheckPenalty => 'Pénalité d\'armure';

  @override
  String get settingsTitle => 'Paramètres';

  @override
  String get settingsLanguage => 'Langue';

  @override
  String get settingsLanguageEn => 'English';

  @override
  String get settingsLanguageFr => 'Français';

  @override
  String get settingsTheme => 'Thème';

  @override
  String get settingsThemeLight => 'Clair';

  @override
  String get settingThemeDark => 'Sombre';

  @override
  String get settingsSync => 'Synchroniser avec le serveur';

  @override
  String get buttonOk => 'OK';

  @override
  String get buttonCancel => 'Annuler';

  @override
  String get buttonSave => 'Enregistrer';

  @override
  String get buttonDelete => 'Supprimer';

  @override
  String get buttonEdit => 'Modifier';

  @override
  String get errorTitle => 'Erreur';

  @override
  String get errorLoadingData => 'Impossible de charger les données';

  @override
  String get errorNetwork => 'Erreur réseau. Vérifiez votre connexion.';

  @override
  String get errorUnknown => 'Une erreur inconnue s\'est produite';

  @override
  String get loadingMessage => 'Chargement...';

  @override
  String get savingMessage => 'Enregistrement...';
}
