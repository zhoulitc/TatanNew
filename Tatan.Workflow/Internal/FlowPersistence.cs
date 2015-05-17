using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Tatan.Common.Exception;
using Tatan.Common.Logging;
using Tatan.Data;
using Tatan.Data.Attribute;
using Tatan.Data.Builder;

namespace Tatan.Workflow.Internal
{
    internal class FlowPersistence : IFlowPersistence
    {
        private readonly IList<string> _activityFields;
        private readonly string _activityTable;
        private readonly Flow _flow;
        private readonly IList<string> _flowFields;
        private readonly string _flowKey;
        private readonly string _flowTable;
        private IDataSource _dataSource;
        private string _sqlCountBusiness;
        private string _sqlCountFlow;
        private string _sqlInsertActivity;
        private string _sqlInsertBusiness;
        private string _sqlInsertFlow;
        private string _sqlUpdateBusiness;
        private string _sqlUpdateFlow;

        public FlowPersistence(Flow flow)
        {
            Assert.ArgumentNotNull("flow", flow);

            _flow = flow;
            _flowTable = DataAttributes.GetTableName<FlowInstance>();
            _flowKey = DataAttributes.GetPrimaryKey<FlowInstance>();
            _flowFields = DataAttributes.GetFieldNames<FlowInstance>();
            _activityTable = DataAttributes.GetTableName<ActivityInstance>();
            _activityFields = DataAttributes.GetFieldNames<ActivityInstance>();
        }

        public bool IsPersistence { get; private set; }

        private string SqlCountBusiness
        {
            get
            {
                if (_sqlInsertBusiness == null)
                {
                    var builder = new SqlBuilder(_flow.BusinessTable, _flow.BusinessKey,
                        null, _dataSource.Provider);
                    _sqlInsertBusiness = builder.GetInsertStatement();
                }
                return _sqlInsertBusiness;
            }
        }

        private string SqlInsertBusiness
        {
            get
            {
                if (_sqlCountBusiness == null)
                {
                    var builder = new SqlBuilder(_flow.BusinessTable, _flow.BusinessKey,
                        _flow.BusinessFields.Keys.ToArray(), _dataSource.Provider);
                    _sqlCountBusiness = builder.GetCountStatement();
                }
                return _sqlCountBusiness;
            }
        }

        private string SqlUpdateBusiness
        {
            get
            {
                if (_sqlUpdateBusiness == null)
                {
                    var builder = new SqlBuilder(_flow.BusinessTable, _flow.BusinessKey,
                        _flow.BusinessFields.Keys.ToArray(), _dataSource.Provider);
                    _sqlUpdateBusiness = builder.GetUpdateStatement();
                }
                return _sqlUpdateBusiness;
            }
        }

        private string SqlCountFlow
        {
            get
            {
                if (_sqlCountFlow == null)
                {
                    var builder = new SqlBuilder(_flowTable, _flowKey,
                        null, _dataSource.Provider);
                    _sqlCountFlow = builder.GetCountStatement();
                }
                return _sqlCountFlow;
            }
        }

        private string SqlInsertFlow
        {
            get
            {
                if (_sqlInsertFlow == null)
                {
                    var builder = new SqlBuilder(_flowTable, _flowKey,
                        _flowFields.ToArray(), _dataSource.Provider);
                    _sqlInsertFlow = builder.GetInsertStatement();
                }
                return _sqlInsertFlow;
            }
        }

        private string SqlUpdateFlow
        {
            get
            {
                if (_sqlUpdateFlow == null)
                {
                    var builder = new SqlBuilder(_flowTable, _flowKey,
                        _flowFields.ToArray(), _dataSource.Provider);
                    _sqlUpdateFlow = builder.GetUpdateStatement();
                }
                return _sqlUpdateFlow;
            }
        }

        private string SqlInsertActivity
        {
            get
            {
                if (_sqlInsertActivity == null)
                {
                    var builder = new SqlBuilder(_activityTable, null,
                        _activityFields.ToArray(), _dataSource.Provider);
                    _sqlInsertActivity = builder.GetInsertStatement();
                }
                return _sqlInsertActivity;
            }
        }

        public void SetDatabase(string providerName, string connectionString)
        {
            try
            {
                _dataSource = DataSource.Connect(providerName, connectionString);
                IsPersistence = true;
            }
            catch (Exception ex)
            {
                Log.Error<FlowPersistence>("save init error.", ex);
            }
        }

        public void SetDatabase(string configName)
        {
            try
            {
                _dataSource = DataSource.Connect(configName);
                IsPersistence = true;
            }
            catch (Exception ex)
            {
                Log.Error<FlowPersistence>("save init error.", ex);
            }
        }

        public event Action<IDataSession, IFlowInstance> HandlerFlow;

        public event Action<IDataSession, IActivityInstance> HandlerActivity;

        public event Action<IDataSession, IFlowInstance> HandlerBusiness;

        internal bool ToDataBase(IFlowInstance instance)
        {
            if (_dataSource == null || instance == null)
                return false;

            return _dataSource.UseSession(instance.Id, session =>
            {
                IDbTransaction tran = null;
                try
                {
                    tran = session.BeginTransaction();

                    //处理流程实体
                    (HandlerFlow ?? OnHandlerFlow)(session, instance);

                    //处理活动实体
                    (HandlerActivity ?? OnHandlerActivity)(session, instance.Track.Current);

                    //处理业务实体
                    (HandlerBusiness ?? OnHandlerBusiness)(session, instance);

                    tran.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    Log.Error<FlowPersistence>("save flow error.", ex);
                    if (tran != null)
                        tran.Rollback();

                    return false;
                }
            });
        }

        private void OnHandlerBusiness(IDataSession session, IFlowInstance instance)
        {
            int count = session.Execute(SqlCountBusiness,
                parameters => { parameters[_flow.BusinessKey] = instance[_flow.BusinessKey]; });

            session.Execute((count > 0) ? SqlUpdateBusiness : SqlInsertBusiness, parameters =>
            {
                foreach (var field in _flow.BusinessFields)
                {
                    parameters[field.Key, field.Value] = instance[field.Key];
                }
            });
        }

        private void OnHandlerFlow(IDataSession session, IFlowInstance instance)
        {
            int count = session.Execute(SqlCountFlow, parameters => { parameters[_flowKey] = instance.Id; });

            IDictionary<string, Tuple<Type, object>> properties = DataAttributes.GetFieldValues(instance, (count > 0));
            properties[_flow.BusinessKey] = Tuple.Create(typeof (string), instance[_flow.BusinessKey]);
            session.Execute((count > 0) ? SqlUpdateFlow : SqlInsertFlow, parameters =>
            {
                foreach (var property in properties)
                {
                    parameters[property.Key, property.Value.Item1] = property.Value.Item2;
                }
            });
        }

        private void OnHandlerActivity(IDataSession session, IActivityInstance instance)
        {
            IDictionary<string, Tuple<Type, object>> properties = DataAttributes.GetFieldValues(instance);
            session.Execute(SqlInsertActivity, parameters =>
            {
                foreach (var property in properties)
                {
                    parameters[property.Key, property.Value.Item1] = property.Value.Item2;
                }
            });
        }
    }
}