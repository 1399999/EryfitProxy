namespace EryfitProxy.Kernel.Tools.DocGen
{
    /// <summary>
    ///     Build filters and rules markdown documentation
    /// </summary>
    public class DocBuilder
    {
        private static readonly string[] HeaderNames = { "Property", "Type", "Description", "DefaultValue" };
        private static readonly string[] HeaderAlignments = { ":-------", ":-------", ":-------", "--------" };
        private readonly DescriptionLineProvider _descriptionLineProvider;
        private readonly RuleConfigParser _ruleConfigParser;

        public DocBuilder(DescriptionLineProvider descriptionLineProvider, RuleConfigParser ruleConfigParser)
        {
            _descriptionLineProvider = descriptionLineProvider;
            _ruleConfigParser = ruleConfigParser;
        }

        public IEnumerable<Filter> RetrieveFilters()
        {
            yield break;
        }

        public void BuildFilter<T>(string directory, List<SearchableItem> items) where T : Filter
        {
            var type = typeof(T);

            BuildFilter(directory, type, items);
        }

        public void BuildFilter(string directory, Type type, List<SearchableItem> items)
        {
            Directory.CreateDirectory(directory);

            var forcedInstance = (Filter) ReflectionHelper.GetForcedInstance<Filter>(type);
            var fileName = Path.Combine(directory, type.Name + ".md");

            using var writer = new StreamWriter(fileName);

            writer.NewLine = "\r\n";

            var filterMetaDataAttribute = type.GetCustomAttribute<FilterMetaDataAttribute>(true);

            if (filterMetaDataAttribute == null)
                throw new InvalidOperationException($"Filter {type.Name} is missing FilterMetaDataAttribute");

            items.Add(new SearchableItem() {
                Title = type.Name.ToCamelCase(),
                Description = filterMetaDataAttribute.LongDescription ?? string.Empty, 
                Category = nameof(Filter),
                FullTypeName = type.FullName!,
                Scope = forcedInstance.FilterScope.ToString().ToCamelCase()
            });

            writer.NewLine = "\r\n";
            writer.WriteLine($"## {type.Name.ToCamelCase()}");
            writer.WriteLine();
            writer.WriteLine("### Description");
            writer.WriteLine();
            writer.WriteLine(filterMetaDataAttribute.LongDescription);
            writer.WriteLine();
            writer.WriteLine("### Evaluation scope");
            writer.WriteLine();
            writer.WriteLine("Evaluation scope defines the timing where this filter will be applied. ");
            writer.WriteLine();

            writer.WriteLine("{.alert .alert-info}");
            writer.WriteLine(":::");
            writer.Write($"**{forcedInstance.FilterScope.ToString().ToCamelCase()}**");
            writer.Write(" ");
            writer.WriteLine(forcedInstance.FilterScope.GetDescription());
            writer.WriteLine(":::");

            writer.WriteLine();
            writer.WriteLine("### YAML configuration name");
            writer.WriteLine();
            writer.WriteLine($"    {type.Name.ToCamelCase()}");
            writer.WriteLine();
            writer.WriteLine("### Settings");
            writer.WriteLine();

            if (forcedInstance.PreMadeFilter) {
                writer.WriteLine("This filter has no specific characteristic");
                writer.WriteLine();
            }

            writer.WriteLine("The following table describes the customizable properties available for this filter: ");
            writer.WriteLine();

            writer.WriteLine("{.property-table .property-table-filter}");
            writer.WriteLine(":::");
            writer.WriteLine(ProduceMarkdownTableLine(HeaderNames));
            writer.WriteLine(ProduceMarkdownTableLine(HeaderAlignments));

            foreach (var filterDescriptionLine in _descriptionLineProvider.EnumerateFilterDescriptions(type)) {
                var array = new[] {
                    filterDescriptionLine.PropertyName,
                    filterDescriptionLine.Type,
                    filterDescriptionLine.Description,
                    filterDescriptionLine.DefaultValue
                };

                writer.WriteLine(ProduceMarkdownTableLine(array));
            }
            writer.WriteLine(":::");

            writer.WriteLine();

            writer.WriteLine("### Example of usage");
            writer.WriteLine();

            var examples = forcedInstance.GetExamples().ToList();

            if (!examples.Any()) {
                writer.WriteLine("This filter has no specific usage example");
                writer.WriteLine();
            }
            else {
                writer.WriteLine("The following examples apply a comment to the filtered exchange");
                writer.WriteLine();

                foreach (var example in examples) {
                    writer.WriteLine($"{example.Description.AddTrailingDotAndUpperCaseFirstChar()}");
                    writer.WriteLine();
                    writer.WriteLine("```yaml");

                    var rule = new Rule(new ApplyCommentAction("filter was applied"),
                        example.Filter);

                    var ruleSet = new RuleSet(rule);

                    var yaml = _ruleConfigParser.GetYamlFromRuleSet(ruleSet);

                    // shift yaml four spaces 

                    var yamlLines = yaml.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                    var output = string.Join("\r\n", yamlLines);

                    writer.WriteLine($"{output}");
                    writer.WriteLine("```");
                    writer.WriteLine();
                    writer.WriteLine();
                }
            }

            writer.WriteLine("### .NET reference");
            writer.WriteLine();
            writer.WriteLine($"View definition of [{type.Name}](https://docs.eryfit.io/api/{type.FullName}.html) for .NET integration.");
            writer.WriteLine();

            writer.WriteLine("### See also");
            writer.WriteLine();

            var seeAlsoFilters = SeeAlsoHelper.GetSeeAlsoFilters(type).ToList();

            if (!seeAlsoFilters.Any()) {
                writer.WriteLine("This filter has no related filter");
                writer.WriteLine();
            }
            else {
                writer.WriteLine("The following filters are related to this filter: ");
                writer.WriteLine();

                foreach (var seeAlsoFilter in seeAlsoFilters) {
                    writer.WriteLine($" - [{seeAlsoFilter.Name.ToCamelCase()}]({seeAlsoFilter.Name.ToCamelCase()})");
                }

                writer.WriteLine();
            }
        }

        public void BuildAction(string directory, Type type, List<SearchableItem> items)
        {
            Directory.CreateDirectory(directory);

            var forcedInstance = ReflectionHelper.GetForcedInstance<Action>(type);
            var fileName = Path.Combine(directory, type.Name + ".md");

            using var writer = new StreamWriter(fileName);

            writer.NewLine = "\r\n";

            var actionMetadataAttribute = type.GetCustomAttribute<ActionMetadataAttribute>(true);

            if (actionMetadataAttribute == null)
                throw new InvalidOperationException($"Action `{type.Name}` is missing FilterMetaDataAttribute");

            items.Add(new SearchableItem()
            {
                Title = type.Name.ToCamelCase(),
                Description = actionMetadataAttribute.LongDescription ?? string.Empty,
                Category = nameof(Action),
                FullTypeName = type.FullName!,
                Scope = forcedInstance.ActionScope.ToString().ToCamelCase()
            });

            writer.NewLine = "\r\n";
            writer.WriteLine($"## {type.Name.ToCamelCase()}");
            writer.WriteLine();
            writer.WriteLine("### Description");
            writer.WriteLine();
            writer.WriteLine(actionMetadataAttribute.LongDescription);
            writer.WriteLine();
            writer.WriteLine("### Evaluation scope");
            writer.WriteLine();
            writer.WriteLine("Evaluation scope defines the timing where this filter will be applied. ");
            writer.WriteLine();

            writer.WriteLine("{.alert .alert-info}");
            writer.WriteLine(":::");
            writer.Write($"**{forcedInstance.ActionScope.ToString().ToCamelCase()}**");
            writer.Write(" ");
            writer.WriteLine(forcedInstance.ActionScope.GetDescription());
            writer.WriteLine(":::");

            writer.WriteLine();
            writer.WriteLine("### YAML configuration name");
            writer.WriteLine();
            writer.WriteLine($"{type.Name.ToCamelCase()}");
            writer.WriteLine();
            writer.WriteLine("### Settings");
            writer.WriteLine();

            if (forcedInstance.IsPremade()) {
                writer.WriteLine("This action has no specific characteristic");
                writer.WriteLine();
            }

            var descriptionLines = _descriptionLineProvider.EnumerateActionDescriptions(type).ToList();

            if (descriptionLines.Any()) {
                writer.WriteLine(
                    "The following table describes the customizable properties available for this action: ");

                writer.WriteLine();

                writer.WriteLine("{.property-table .property-table-action}");
                writer.WriteLine(":::");
                writer.WriteLine(ProduceMarkdownTableLine(HeaderNames));
                writer.WriteLine(ProduceMarkdownTableLine(HeaderAlignments));

                foreach (var actionDescriptionLine in _descriptionLineProvider.EnumerateActionDescriptions(type)) {
                    var array = new[] {
                        actionDescriptionLine.PropertyName,
                        actionDescriptionLine.Type,
                        actionDescriptionLine.Description,
                        actionDescriptionLine.DefaultValue
                    };

                    writer.WriteLine(ProduceMarkdownTableLine(array));
                }

                writer.WriteLine();
                writer.WriteLine(":::");
            }

            writer.WriteLine("### Example of usage");
            writer.WriteLine();

            var examples = forcedInstance.GetExamples().ToList();

            if (!examples.Any()) {
                writer.WriteLine("This filter has no specific usage example");
                writer.WriteLine();
            }
            else {
                writer.WriteLine("The following examples apply this action to any exchanges");
                writer.WriteLine();

                foreach (var example in examples) {
                    writer.WriteLine($"{example.Description.AddTrailingDotAndUpperCaseFirstChar()}");
                    writer.WriteLine();
                    writer.WriteLine("```yaml");

                    var rule = new Rule(example.Action, AnyFilter.Default);

                    var ruleSet = new RuleSet(rule);

                    var yaml = _ruleConfigParser.GetYamlFromRuleSet(ruleSet);

                    // shift yaml four spaces 

                    var yamlLines = yaml.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                    var output = string.Join("\r\n", yamlLines);

                    writer.WriteLine($"{output}");
                    writer.WriteLine("```");
                    writer.WriteLine();
                    writer.WriteLine();
                }
            }

            writer.WriteLine();

            writer.WriteLine("### .NET reference");
            writer.WriteLine();
            writer.WriteLine($"View definition of [{type.Name}](https://docs.eryfit.io/api/{type.FullName}.html) for .NET integration.");
            writer.WriteLine();

            writer.WriteLine("### See also");
            writer.WriteLine();

            var seeAlsoActions = SeeAlsoHelper.GetSeeAlsoActions(type).ToList();

            if (!seeAlsoActions.Any()) {
                writer.WriteLine("This action has no related action");
                writer.WriteLine();
            }
            else {
                writer.WriteLine("The following actions are related to this action: ");
                writer.WriteLine();

                foreach (var seeAlsoAction in seeAlsoActions) {
                    writer.WriteLine($" - [{seeAlsoAction.Name.ToCamelCase()}]({seeAlsoAction.Name.ToCamelCase()})");
                }

                writer.WriteLine();
            }
        }

        private static string ProduceMarkdownTableLine(IEnumerable<string> columns)
        {
            return $"| {string.Join(" | ", columns)} |";
        }
    }
}
