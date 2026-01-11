enum CompatibilityType { synergy, antagonistic, neutral }

class CompatibilityResult {
  final CompatibilityType type;
  final String message;

  CompatibilityResult({required this.type, required this.message});
}