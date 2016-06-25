
package nz.co.API.Technical.API1;

import java.util.ArrayList;
import java.util.List;
import javax.annotation.Generated;
import javax.validation.Valid;
import com.fasterxml.jackson.annotation.JsonInclude;
import com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.annotation.JsonPropertyOrder;
import org.apache.commons.lang3.builder.EqualsBuilder;
import org.apache.commons.lang3.builder.HashCodeBuilder;
import org.apache.commons.lang3.builder.ToStringBuilder;


/**
 * ResponseInfo
 * <p>
 * Response  Info
 * 
 */
@JsonInclude(JsonInclude.Include.NON_NULL)
@Generated("org.jsonschema2pojo")
@JsonPropertyOrder({
    "id",
    "code",
    "message",
    "description",
    "reasons",
    "status"
})
public class ResponseInfo {

    /**
     * The id field is used to correlate errors across systems.
     * The id will be generated on the server and can be logged in the front end to allow a full end to end view of the sceneario involving the exception across multiple systems
     * 
     */
    @JsonProperty("id")
    private String id;
    /**
     * The code field is not the copy of the HTTP response.
     * 
     * The code field is likely to become an enum value with values that are common across all APIs.
     * 
     */
    @JsonProperty("code")
    private String code;
    /**
     * Note that this value is not intended to be a customer facing message.  Customer facing messages should be derived from the through a mapping from the code value.  The code should be passed to a content management system to determine the content to display to the end user.  This allows the content to be managed separately from the life cycle of the application functionality.
     * 
     */
    @JsonProperty("message")
    private String message;
    @JsonProperty("description")
    private String description;
    /**
     * 
     * 
     */
    @JsonProperty("reasons")
    @Valid
    private List<Reasons> reasons = new ArrayList<Reasons>();
    /**
     * HTTPStatusCategory
     * <p>
     * Indicates the general category of failure
     *  4XX_CLIENT_ERROR:
     *  5XX_SERVER_ERROR:
     * 
     */
    @JsonProperty("status")
    private HTTPStatusCategory status;

    /**
     * The id field is used to correlate errors across systems.
     * The id will be generated on the server and can be logged in the front end to allow a full end to end view of the sceneario involving the exception across multiple systems
     * 
     * @return
     *     The id
     */
    @JsonProperty("id")
    public String getId() {
        return id;
    }

    /**
     * The id field is used to correlate errors across systems.
     * The id will be generated on the server and can be logged in the front end to allow a full end to end view of the sceneario involving the exception across multiple systems
     * 
     * @param id
     *     The id
     */
    @JsonProperty("id")
    public void setId(String id) {
        this.id = id;
    }

    /**
     * The code field is not the copy of the HTTP response.
     * 
     * The code field is likely to become an enum value with values that are common across all APIs.
     * 
     * @return
     *     The code
     */
    @JsonProperty("code")
    public String getCode() {
        return code;
    }

    /**
     * The code field is not the copy of the HTTP response.
     * 
     * The code field is likely to become an enum value with values that are common across all APIs.
     * 
     * @param code
     *     The code
     */
    @JsonProperty("code")
    public void setCode(String code) {
        this.code = code;
    }

    /**
     * Note that this value is not intended to be a customer facing message.  Customer facing messages should be derived from the through a mapping from the code value.  The code should be passed to a content management system to determine the content to display to the end user.  This allows the content to be managed separately from the life cycle of the application functionality.
     * 
     * @return
     *     The message
     */
    @JsonProperty("message")
    public String getMessage() {
        return message;
    }

    /**
     * Note that this value is not intended to be a customer facing message.  Customer facing messages should be derived from the through a mapping from the code value.  The code should be passed to a content management system to determine the content to display to the end user.  This allows the content to be managed separately from the life cycle of the application functionality.
     * 
     * @param message
     *     The message
     */
    @JsonProperty("message")
    public void setMessage(String message) {
        this.message = message;
    }

    /**
     * 
     * @return
     *     The description
     */
    @JsonProperty("description")
    public String getDescription() {
        return description;
    }

    /**
     * 
     * @param description
     *     The description
     */
    @JsonProperty("description")
    public void setDescription(String description) {
        this.description = description;
    }

    /**
     * 
     * 
     * @return
     *     The reasons
     */
    @JsonProperty("reasons")
    public List<Reasons> getReasons() {
        return reasons;
    }

    /**
     * 
     * 
     * @param reasons
     *     The reasons
     */
    @JsonProperty("reasons")
    public void setReasons(List<Reasons> reasons) {
        this.reasons = reasons;
    }

    /**
     * HTTPStatusCategory
     * <p>
     * Indicates the general category of failure
     *  4XX_CLIENT_ERROR:
     *  5XX_SERVER_ERROR:
     * 
     * @return
     *     The status
     */
    @JsonProperty("status")
    public HTTPStatusCategory getStatus() {
        return status;
    }

    /**
     * HTTPStatusCategory
     * <p>
     * Indicates the general category of failure
     *  4XX_CLIENT_ERROR:
     *  5XX_SERVER_ERROR:
     * 
     * @param status
     *     The status
     */
    @JsonProperty("status")
    public void setStatus(HTTPStatusCategory status) {
        this.status = status;
    }

    @Override
    public String toString() {
        return ToStringBuilder.reflectionToString(this);
    }

    @Override
    public int hashCode() {
        return new HashCodeBuilder().append(id).append(code).append(message).append(description).append(reasons).append(status).toHashCode();
    }

    @Override
    public boolean equals(Object other) {
        if (other == this) {
            return true;
        }
        if ((other instanceof ResponseInfo) == false) {
            return false;
        }
        ResponseInfo rhs = ((ResponseInfo) other);
        return new EqualsBuilder().append(id, rhs.id).append(code, rhs.code).append(message, rhs.message).append(description, rhs.description).append(reasons, rhs.reasons).append(status, rhs.status).isEquals();
    }

}
