using Prism.Events;

namespace ViewModels.Events
{
    public class LanguageAddedEvent : PubSubEvent<LingvaViewModel> { }
    public class LanguageDeletedEvent : PubSubEvent<LingvaViewModel> { }
}
