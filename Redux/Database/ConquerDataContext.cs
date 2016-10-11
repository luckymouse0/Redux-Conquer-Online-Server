using Redux.Database.Domain;
using Redux.Database.Repositories;


namespace Redux.Database
{
    public class ConquerDataContext
    {

        private AccountRepository _accountRepository;
        private CharacterRepository _characterRepository;
        private ItemInfoRepository _itemsRepository;
        private ItemAdditionRepository _itemAdditionRepository;
        private ItemRepository _itemRepository;
        private ProficiencyRepository _proficiencyRepository;
        private SkillRepository _skillRepository;
        private MagicTypeRepository _magicTypeRepository;
        private PortalRepository _portalRepository;
        private PassageRepository _passageRepository;
        private MapRepository _mapRepository;
        private NpcRepository _npcRepository;
        private ItemLogRepository _itemLogRepository;
        private ChatLogRepository _chatLogRepository;
        private MonstertypeRepository _monstertypeRepository;
        private SpawnRepository _spawnRepository;
        private StatRepository _statRepository;
        private DropRuleRepository _dropRuleRepository;
        private ShopItemRepository _shopItemRepository;
        private AssociateRepository _associateRepository;
        private GuildRepository _guildRepository;
        private GuildAttrRepository _guildAttrRepository;
        private SobRepository _SOBRepository;
        private EventsRepository _EventsRepository;
        private NobilityRepository _NobilityRepository;
        private RebornRepository _RebornRepository;
        private MineDropRepository _MineDropRepository;
        private LevExpRepository _LevExpRepository;
        private BugReportRepository _bugReportRepository;
        private TaskRepository _taskRepository;

        public MineDropRepository MineDrop
        {
            get { return _MineDropRepository ?? (_MineDropRepository = new MineDropRepository()); }
        }

        public NobilityRepository Nobility
        {
            get { return _NobilityRepository ?? (_NobilityRepository = new NobilityRepository()); }
        }

        public EventsRepository Events
        {
            get { return _EventsRepository ?? (_EventsRepository = new EventsRepository()); }
        }
        public SobRepository SOB
        {
            get { return _SOBRepository ?? (_SOBRepository = new SobRepository()); }
        }
        public GuildRepository Guilds
        {
            get { return _guildRepository ?? (_guildRepository = new GuildRepository()); }
        }

        public GuildAttrRepository GuildAttributes
        {
            get { return _guildAttrRepository ?? (_guildAttrRepository = new GuildAttrRepository()); }
        }

        public AccountRepository Accounts
        {
            get { return _accountRepository ?? (_accountRepository = new AccountRepository()); }
        }
        public CharacterRepository Characters
        {
            get { return _characterRepository ?? (_characterRepository = new CharacterRepository()); }
        }
        public AssociateRepository Associates
        {
            get { return _associateRepository ?? (_associateRepository = new AssociateRepository()); }
        }
        public ItemInfoRepository ItemInformation
        {
            get { return _itemsRepository ?? (_itemsRepository = new ItemInfoRepository()); }
        }
        public ItemAdditionRepository ItemAddition
        {
            get { return _itemAdditionRepository ?? (_itemAdditionRepository = new ItemAdditionRepository()); }
        }
        public ItemRepository Items
        {
            get { return _itemRepository ?? (_itemRepository = new ItemRepository()); }
        }
        public SkillRepository Skills
        {
            get { return _skillRepository ?? (_skillRepository = new SkillRepository()); }
        }
        public ProficiencyRepository Proficiencies
        {
            get { return _proficiencyRepository ?? (_proficiencyRepository = new ProficiencyRepository()); }
        }
        public MagicTypeRepository MagicType
        {
            get { return _magicTypeRepository ?? (_magicTypeRepository = new MagicTypeRepository()); }
        }
        public PortalRepository Portals
        {
            get { return _portalRepository ?? (_portalRepository = new PortalRepository()); }
        }
        public PassageRepository Passages
        {
            get { return _passageRepository ?? (_passageRepository = new PassageRepository()); }
        }
        public MapRepository Maps
        {
            get { return _mapRepository ?? (_mapRepository = new MapRepository()); }
        }
        public NpcRepository Npcs
        {
            get { return _npcRepository ?? (_npcRepository = new NpcRepository()); }
        }
        public ItemLogRepository ItemLogs
        {
            get { return _itemLogRepository ?? (_itemLogRepository = new ItemLogRepository()); }
        }
        public ChatLogRepository ChatLogs
        {
            get { return _chatLogRepository ?? (_chatLogRepository = new ChatLogRepository()); }
        }
        public MonstertypeRepository Monstertype
        {
            get { return _monstertypeRepository ?? (_monstertypeRepository = new MonstertypeRepository()); }
        }
        public SpawnRepository Spawns
        {
            get { return _spawnRepository ?? (_spawnRepository = new SpawnRepository()); }
        }
        public StatRepository Stats
        {
            get { return _statRepository ?? (_statRepository = new StatRepository()); }
        }
        public DropRuleRepository DropRules
        {
            get {return _dropRuleRepository ?? (_dropRuleRepository = new DropRuleRepository());}
        }
        public ShopItemRepository ShopItems
        {
            get { return _shopItemRepository ?? (_shopItemRepository = new ShopItemRepository()); }
        }
        public RebornRepository RebornPaths
        {
            get { return _RebornRepository ?? (_RebornRepository = new RebornRepository()); }
        }
        public LevExpRepository LevelExp
        {
            get { return _LevExpRepository ?? (_LevExpRepository = new LevExpRepository()); }
        }
        public BugReportRepository BugReports
        {
            get { return _bugReportRepository ?? (_bugReportRepository = new BugReportRepository()); }
        }
        public TaskRepository Tasks
        {
            get { return _taskRepository ?? (_taskRepository = new TaskRepository()); }
        }

    }
}
