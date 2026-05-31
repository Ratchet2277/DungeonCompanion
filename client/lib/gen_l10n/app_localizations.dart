import 'dart:async';
import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart' as intl;

import 'app_localizations_en.dart';
import 'app_localizations_fr.dart';

abstract class AppLocalizations {
  AppLocalizations(String locale) : localeName = intl.Intl.canonicalizedLocale(locale.toString());

  final String localeName;

  static AppLocalizations of(BuildContext context) => Localizations.of<AppLocalizations>(context, AppLocalizations)!;

  static const LocalizationsDelegate<AppLocalizations> delegate = _AppLocalizationsDelegate();

  static List<LocalizationsDelegate<dynamic>> get localizationsDelegates => <LocalizationsDelegate<dynamic>>[
    delegate,
  ];

  static const List<Locale> supportedLocales = <Locale>[
    Locale('en'),
    Locale('fr'),
  ];

  String get appTitle;
  String get appSubtitle;
  String get navigationHome;
  String get navigationCharacters;
  String get navigationSpells;
  String get navigationSkills;
  String get navigationSettings;
  String get charactersTitle;
  String get charactersEmpty;
  String get charactersCreateButton;
  String get characterLevel;
  String get characterExperience;
  String get characterHealth;
  String get characterAttributes;
  String get characterSpells;
  String get characterSkills;
  String get spellsTitle;
  String get spellsEmpty;
  String get spellLevel;
  String get spellSchool;
  String get spellCastingTime;
  String get spellRange;
  String get spellComponent;
  String get spellDuration;
  String get spellSavingThrow;
  String get spellDescription;
  String get skillsTitle;
  String get skillsEmpty;
  String get skillKeyAbility;
  String get skillTrainedOnly;
  String get skillArmorCheckPenalty;
  String get settingsTitle;
  String get settingsLanguage;
  String get settingsLanguageEn;
  String get settingsLanguageFr;
  String get settingsTheme;
  String get settingsThemeLight;
  String get settingThemeDark;
  String get settingsSync;
  String get buttonOk;
  String get buttonCancel;
  String get buttonSave;
  String get buttonDelete;
  String get buttonEdit;
  String get errorTitle;
  String get errorLoadingData;
  String get errorNetwork;
  String get errorUnknown;
  String get loadingMessage;
  String get savingMessage;
}

class _AppLocalizationsDelegate extends LocalizationsDelegate<AppLocalizations> {
  const _AppLocalizationsDelegate();

  @override
  Future<AppLocalizations> load(Locale locale) {
    return SynchronousFuture<AppLocalizations>(_initializeMessages(locale));
  }

  @override
  bool isSupported(Locale locale) => <String>['en', 'fr'].contains(locale.languageCode);

  @override
  bool shouldReload(_AppLocalizationsDelegate old) => false;

  AppLocalizations _initializeMessages(Locale locale) {
    final String localeName = intl.Intl.canonicalizedLocale(locale.toString());
    intl.Intl.defaultLocale = localeName;

    if (locale.languageCode == 'fr') {
      return AppLocalizationsFr();
    }
    return AppLocalizationsEn();
  }
}
