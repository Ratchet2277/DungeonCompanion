import 'dart:async';

import 'package:flutter/foundation.dart';
import 'package:flutter/widgets.dart';
import 'package:flutter_localizations/flutter_localizations.dart';
import 'package:intl/intl.dart' as intl;

import 'app_localizations_en.dart';
import 'app_localizations_fr.dart';

// ignore_for_file: type=lint

/// Callers can lookup localized strings with an instance of AppLocalizations
/// returned by `AppLocalizations.of(context)`.
///
/// Applications need to include `AppLocalizations.delegate()` in their app's
/// `localizationDelegates` list, and the locales they support in the app's
/// `supportedLocales` list. For example:
///
/// ```dart
/// import 'gen_l10n/app_localizations.dart';
///
/// return MaterialApp(
///   localizationsDelegates: AppLocalizations.localizationsDelegates,
///   supportedLocales: AppLocalizations.supportedLocales,
///   home: MyApplicationHome(),
/// );
/// ```
///
/// ## Update pubspec.yaml
///
/// Please make sure to update your pubspec.yaml to include the following
/// packages:
///
/// ```yaml
/// dependencies:
///   # Internationalization support.
///   flutter_localizations:
///     sdk: flutter
///   intl: any # Use the pinned version from flutter_localizations
///
///   # Rest of dependencies
/// ```
///
/// ## iOS Applications
///
/// iOS applications define key application metadata, including supported
/// locales, in an Info.plist file that is built into the application bundle.
/// To configure the locales supported by your app, you’ll need to edit this
/// file.
///
/// First, open your project’s ios/Runner.xcworkspace Xcode workspace file.
/// Then, in the Project Navigator, open the Info.plist file under the Runner
/// project’s Runner folder.
///
/// Next, select the Information Property List item, select Add Item from the
/// Editor menu, then select Localizations from the pop-up menu.
///
/// Select and expand the newly-created Localizations item then, for each
/// locale your application supports, add a new item and select the locale
/// you wish to add from the pop-up menu in the Value field. This list should
/// be consistent with the languages listed in the AppLocalizations.supportedLocales
/// property.
abstract class AppLocalizations {
  AppLocalizations(String locale)
    : localeName = intl.Intl.canonicalizedLocale(locale.toString());

  final String localeName;

  static AppLocalizations? of(BuildContext context) {
    return Localizations.of<AppLocalizations>(context, AppLocalizations);
  }

  static const LocalizationsDelegate<AppLocalizations> delegate =
      _AppLocalizationsDelegate();

  /// A list of this localizations delegate along with the default localizations
  /// delegates.
  ///
  /// Returns a list of localizations delegates containing this delegate along with
  /// GlobalMaterialLocalizations.delegate, GlobalCupertinoLocalizations.delegate,
  /// and GlobalWidgetsLocalizations.delegate.
  ///
  /// Additional delegates can be added by appending to this list in
  /// MaterialApp. This list does not have to be used at all if a custom list
  /// of delegates is preferred or required.
  static const List<LocalizationsDelegate<dynamic>> localizationsDelegates =
      <LocalizationsDelegate<dynamic>>[
        delegate,
        GlobalMaterialLocalizations.delegate,
        GlobalCupertinoLocalizations.delegate,
        GlobalWidgetsLocalizations.delegate,
      ];

  /// A list of this localizations delegate's supported locales.
  static const List<Locale> supportedLocales = <Locale>[
    Locale('en'),
    Locale('fr'),
  ];

  /// No description provided for @appTitle.
  ///
  /// In en, this message translates to:
  /// **'Dungeon Companion'**
  String get appTitle;

  /// No description provided for @appSubtitle.
  ///
  /// In en, this message translates to:
  /// **'Manage your D&D characters'**
  String get appSubtitle;

  /// No description provided for @navigationHome.
  ///
  /// In en, this message translates to:
  /// **'Home'**
  String get navigationHome;

  /// No description provided for @navigationCharacters.
  ///
  /// In en, this message translates to:
  /// **'Characters'**
  String get navigationCharacters;

  /// No description provided for @navigationSpells.
  ///
  /// In en, this message translates to:
  /// **'Spells'**
  String get navigationSpells;

  /// No description provided for @navigationSkills.
  ///
  /// In en, this message translates to:
  /// **'Skills'**
  String get navigationSkills;

  /// No description provided for @navigationSettings.
  ///
  /// In en, this message translates to:
  /// **'Settings'**
  String get navigationSettings;

  /// No description provided for @charactersTitle.
  ///
  /// In en, this message translates to:
  /// **'My Characters'**
  String get charactersTitle;

  /// No description provided for @charactersEmpty.
  ///
  /// In en, this message translates to:
  /// **'No characters yet. Create your first character!'**
  String get charactersEmpty;

  /// No description provided for @charactersCreateButton.
  ///
  /// In en, this message translates to:
  /// **'New Character'**
  String get charactersCreateButton;

  /// No description provided for @characterLevel.
  ///
  /// In en, this message translates to:
  /// **'Level'**
  String get characterLevel;

  /// No description provided for @characterExperience.
  ///
  /// In en, this message translates to:
  /// **'Experience'**
  String get characterExperience;

  /// No description provided for @characterHealth.
  ///
  /// In en, this message translates to:
  /// **'Health'**
  String get characterHealth;

  /// No description provided for @characterAttributes.
  ///
  /// In en, this message translates to:
  /// **'Attributes'**
  String get characterAttributes;

  /// No description provided for @characterSpells.
  ///
  /// In en, this message translates to:
  /// **'Spells'**
  String get characterSpells;

  /// No description provided for @characterSkills.
  ///
  /// In en, this message translates to:
  /// **'Skills'**
  String get characterSkills;

  /// No description provided for @spellsTitle.
  ///
  /// In en, this message translates to:
  /// **'Spells'**
  String get spellsTitle;

  /// No description provided for @spellsEmpty.
  ///
  /// In en, this message translates to:
  /// **'No spells available'**
  String get spellsEmpty;

  /// No description provided for @spellLevel.
  ///
  /// In en, this message translates to:
  /// **'Level'**
  String get spellLevel;

  /// No description provided for @spellSchool.
  ///
  /// In en, this message translates to:
  /// **'School'**
  String get spellSchool;

  /// No description provided for @spellCastingTime.
  ///
  /// In en, this message translates to:
  /// **'Casting Time'**
  String get spellCastingTime;

  /// No description provided for @spellRange.
  ///
  /// In en, this message translates to:
  /// **'Range'**
  String get spellRange;

  /// No description provided for @spellComponent.
  ///
  /// In en, this message translates to:
  /// **'Component'**
  String get spellComponent;

  /// No description provided for @spellDuration.
  ///
  /// In en, this message translates to:
  /// **'Duration'**
  String get spellDuration;

  /// No description provided for @spellSavingThrow.
  ///
  /// In en, this message translates to:
  /// **'Saving Throw'**
  String get spellSavingThrow;

  /// No description provided for @spellDescription.
  ///
  /// In en, this message translates to:
  /// **'Description'**
  String get spellDescription;

  /// No description provided for @skillsTitle.
  ///
  /// In en, this message translates to:
  /// **'Skills'**
  String get skillsTitle;

  /// No description provided for @skillsEmpty.
  ///
  /// In en, this message translates to:
  /// **'No skills available'**
  String get skillsEmpty;

  /// No description provided for @skillKeyAbility.
  ///
  /// In en, this message translates to:
  /// **'Key Ability'**
  String get skillKeyAbility;

  /// No description provided for @skillTrainedOnly.
  ///
  /// In en, this message translates to:
  /// **'Trained Only'**
  String get skillTrainedOnly;

  /// No description provided for @skillArmorCheckPenalty.
  ///
  /// In en, this message translates to:
  /// **'Armor Check Penalty'**
  String get skillArmorCheckPenalty;

  /// No description provided for @settingsTitle.
  ///
  /// In en, this message translates to:
  /// **'Settings'**
  String get settingsTitle;

  /// No description provided for @settingsLanguage.
  ///
  /// In en, this message translates to:
  /// **'Language'**
  String get settingsLanguage;

  /// No description provided for @settingsLanguageEn.
  ///
  /// In en, this message translates to:
  /// **'English'**
  String get settingsLanguageEn;

  /// No description provided for @settingsLanguageFr.
  ///
  /// In en, this message translates to:
  /// **'Français'**
  String get settingsLanguageFr;

  /// No description provided for @settingsTheme.
  ///
  /// In en, this message translates to:
  /// **'Theme'**
  String get settingsTheme;

  /// No description provided for @settingsThemeLight.
  ///
  /// In en, this message translates to:
  /// **'Light'**
  String get settingsThemeLight;

  /// No description provided for @settingThemeDark.
  ///
  /// In en, this message translates to:
  /// **'Dark'**
  String get settingThemeDark;

  /// No description provided for @settingsSync.
  ///
  /// In en, this message translates to:
  /// **'Sync with Server'**
  String get settingsSync;

  /// No description provided for @buttonOk.
  ///
  /// In en, this message translates to:
  /// **'OK'**
  String get buttonOk;

  /// No description provided for @buttonCancel.
  ///
  /// In en, this message translates to:
  /// **'Cancel'**
  String get buttonCancel;

  /// No description provided for @buttonSave.
  ///
  /// In en, this message translates to:
  /// **'Save'**
  String get buttonSave;

  /// No description provided for @buttonDelete.
  ///
  /// In en, this message translates to:
  /// **'Delete'**
  String get buttonDelete;

  /// No description provided for @buttonEdit.
  ///
  /// In en, this message translates to:
  /// **'Edit'**
  String get buttonEdit;

  /// No description provided for @errorTitle.
  ///
  /// In en, this message translates to:
  /// **'Error'**
  String get errorTitle;

  /// No description provided for @errorLoadingData.
  ///
  /// In en, this message translates to:
  /// **'Failed to load data'**
  String get errorLoadingData;

  /// No description provided for @errorNetwork.
  ///
  /// In en, this message translates to:
  /// **'Network error. Check your connection.'**
  String get errorNetwork;

  /// No description provided for @errorUnknown.
  ///
  /// In en, this message translates to:
  /// **'An unknown error occurred'**
  String get errorUnknown;

  /// No description provided for @loadingMessage.
  ///
  /// In en, this message translates to:
  /// **'Loading...'**
  String get loadingMessage;

  /// No description provided for @savingMessage.
  ///
  /// In en, this message translates to:
  /// **'Saving...'**
  String get savingMessage;
}

class _AppLocalizationsDelegate
    extends LocalizationsDelegate<AppLocalizations> {
  const _AppLocalizationsDelegate();

  @override
  Future<AppLocalizations> load(Locale locale) {
    return SynchronousFuture<AppLocalizations>(lookupAppLocalizations(locale));
  }

  @override
  bool isSupported(Locale locale) =>
      <String>['en', 'fr'].contains(locale.languageCode);

  @override
  bool shouldReload(_AppLocalizationsDelegate old) => false;
}

AppLocalizations lookupAppLocalizations(Locale locale) {
  // Lookup logic when only language code is specified.
  switch (locale.languageCode) {
    case 'en':
      return AppLocalizationsEn();
    case 'fr':
      return AppLocalizationsFr();
  }

  throw FlutterError(
    'AppLocalizations.delegate failed to load unsupported locale "$locale". This is likely '
    'an issue with the localizations generation tool. Please file an issue '
    'on GitHub with a reproducible sample app and the gen-l10n configuration '
    'that was used.',
  );
}
