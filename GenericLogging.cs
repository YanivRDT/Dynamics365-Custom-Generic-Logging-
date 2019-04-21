using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using System.ServiceModel;

/// <summary>
/// Use to log trace and exception entries from Plug-ins, Custom Workflow Activities and 
/// Extranl components cosuming Dynamics 365 SDK
/// </summary>
//namespace Extending.Dyn365.v9.Plugin.Plugins
//{
    internal static class GenericLogging
    {
        #region members

        internal enum LogType
        {
            Trace = 221490000, Exception = 221490001
        }

        #endregion

        /// <summary>
        /// Log entry 
        /// </summary>
        /// <param name="logEntry"></param>
        /// <param name="type"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        public static string Log(
            Log logEntry,
            LogType type,
            IOrganizationService service)
        {
            string result = string.Empty;

            try
            {
                OrganizationRequest logRequest = new OrganizationRequest("dyn_Log")
                {
                    Parameters = AssembleLogParams(logEntry, type)
                };

                //set ExecuteMultipleRequest request to wrap log Exception Request
                //this is required to escape the Plugin transaction which if rolled back, 
                //will delete the log entry record
                ExecuteMultipleRequest executeMultipleRequest = new ExecuteMultipleRequest()
                {
                    Requests = new OrganizationRequestCollection(),
                    Settings = new ExecuteMultipleSettings()
                    {
                        ContinueOnError = true,
                        ReturnResponses = true
                    }
                };

                //add request to multiple requests collection
                executeMultipleRequest.Requests.Add(logRequest);

                //execute ExecuteMultipleRequest
                ExecuteMultipleResponse resp = (ExecuteMultipleResponse)service.Execute(executeMultipleRequest);

                //parse ExecuteMultipleRequest response 
                foreach (var responseItem in resp.Responses)
                {
                    //valid response returned
                    if (responseItem.Response != null)
                    {
                        OrganizationResponse response = (OrganizationResponse)responseItem.Response;
                        result = (string)response["referenceToken"];
                    }
                    //error occurred
                    else if (responseItem.Fault != null)
                    {
                        result = responseItem.Fault.Message;
                    }
                }
            }
            catch (FaultException<OrganizationServiceFault> fault)
            {
                result = fault.Message;
            }

            return result;
        }

        /// <summary>
        /// Assemble Log parameter object 
        /// </summary>
        /// <returns></returns>
        public static ParameterCollection AssembleLogParams(Log logEntry, LogType type)
        {
            ParameterCollection result = new ParameterCollection()
            {
                ["type"] = (int)type
            };

            if (logEntry.title != null)
            {
                result.Add("title", logEntry.title);
            }
            else
            {
                result.Add("title", "Title unspecified");
            }

            if (logEntry.description != null)
            {
                result.Add("description", logEntry.description);
            }

            if (logEntry.relatedBusinessRecordId != null)
            {
                result.Add("relatedBusinessRecordId", logEntry.relatedBusinessRecordId);
            }

            if (logEntry.relatedBusinessRecordURL != null)
            {
                result.Add("relatedBusinessRecordURL", logEntry.relatedBusinessRecordURL);
            }

            if (logEntry.relatedUser != null)
            {
                result.Add("relatedUser", logEntry.relatedUser);
            }

            if (logEntry.errorMessage != null)
            {
                result.Add("errorMessage", logEntry.errorMessage);
            }

            if (logEntry.stackTrace != null)
            {
                result.Add("stackTrace", logEntry.stackTrace);
            }

            return result;
        }
    }

    /// <summary>
    /// Define Log entry
    /// </summary>
    public partial class Log : Microsoft.Xrm.Sdk.OrganizationRequest
    {
        public string title
        {
            get
            {
                if (this.Parameters.Contains("title"))
                {
                    return ((string)(this.Parameters["title"]));
                }
                else
                {
                    return default(string);
                }
            }
            set
            {
                this.Parameters["title"] = value;
            }
        }

        public string description
        {
            get
            {
                if (this.Parameters.Contains("description"))
                {
                    return ((string)(this.Parameters["description"]));
                }
                else
                {
                    return default(string);
                }
            }
            set
            {
                this.Parameters["description"] = value;
            }
        }

        public int type
        {
            get
            {
                if (this.Parameters.Contains("type"))
                {
                    return ((int)(this.Parameters["type"]));
                }
                else
                {
                    return default(int);
                }
            }
            set
            {
                this.Parameters["type"] = value;
            }
        }

        public string errorMessage
        {
            get
            {
                if (this.Parameters.Contains("errorMessage"))
                {
                    return ((string)(this.Parameters["errorMessage"]));
                }
                else
                {
                    return default(string);
                }
            }
            set
            {
                this.Parameters["errorMessage"] = value;
            }
        }

        public string stackTrace
        {
            get
            {
                if (this.Parameters.Contains("stackTrace"))
                {
                    return ((string)(this.Parameters["stackTrace"]));
                }
                else
                {
                    return default(string);
                }
            }
            set
            {
                this.Parameters["stackTrace"] = value;
            }
        }

        public string relatedBusinessRecordId
        {
            get
            {
                if (this.Parameters.Contains("relatedBusinessRecordId"))
                {
                    return ((string)(this.Parameters["relatedBusinessRecordId"]));
                }
                else
                {
                    return default(string);
                }
            }
            set
            {
                this.Parameters["relatedBusinessRecordId"] = value;
            }
        }

        public string relatedBusinessRecordURL
        {
            get
            {
                if (this.Parameters.Contains("relatedBusinessRecordURL"))
                {
                    return ((string)(this.Parameters["relatedBusinessRecordURL"]));
                }
                else
                {
                    return default(string);
                }
            }
            set
            {
                this.Parameters["relatedBusinessRecordURL"] = value;
            }
        }

        public Microsoft.Xrm.Sdk.EntityReference relatedUser
        {
            get
            {
                if (this.Parameters.Contains("relatedUser"))
                {
                    return ((Microsoft.Xrm.Sdk.EntityReference)(this.Parameters["relatedUser"]));
                }
                else
                {
                    return default(Microsoft.Xrm.Sdk.EntityReference);
                }
            }
            set
            {
                this.Parameters["relatedUser"] = value;
            }
        }

        public Microsoft.Xrm.Sdk.EntityReference Target
        {
            get
            {
                if (this.Parameters.Contains("Target"))
                {
                    return ((Microsoft.Xrm.Sdk.EntityReference)(this.Parameters["Target"]));
                }
                else
                {
                    return default(Microsoft.Xrm.Sdk.EntityReference);
                }
            }
            set
            {
                this.Parameters["Target"] = value;
            }
        }
        public Log()
        {
            this.RequestName = "dyn_Log";
            this.title = default(string);
        }
    }
//}
