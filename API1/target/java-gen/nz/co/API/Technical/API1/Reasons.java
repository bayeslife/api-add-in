
package nz.co.API.Technical.API1;

import javax.annotation.Generated;
import javax.validation.constraints.NotNull;
import com.fasterxml.jackson.annotation.JsonInclude;
import com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.annotation.JsonPropertyOrder;
import org.apache.commons.lang3.builder.EqualsBuilder;
import org.apache.commons.lang3.builder.HashCodeBuilder;
import org.apache.commons.lang3.builder.ToStringBuilder;


/**
 * Reasons
 * <p>
 * 
 * 
 */
@JsonInclude(JsonInclude.Include.NON_NULL)
@Generated("org.jsonschema2pojo")
@JsonPropertyOrder({
    "field",
    "message",
    "code"
})
public class Reasons {

    /**
     * The field whose value is not valid.
     * (Required)
     * 
     */
    @JsonProperty("field")
    @NotNull
    private String field;
    /**
     * The reason message is to be used by a developer trying to understand what a code means.
     * 
     * The message is not expected to be rendered to the end user.
     * (Required)
     * 
     */
    @JsonProperty("message")
    @NotNull
    private String message;
    /**
     * ReasonCode
     * <p>
     * The reasonCode is expected to be used by the client to go to a content management system to determine some content that is to be displayed to the end user.
     * FIELD_REQUIRED:A field that is required was not provided.
     * INVALID_FORMAT:A field was provided but is in the wrong format.
     * 
     */
    @JsonProperty("code")
    private ReasonCode code;

    /**
     * The field whose value is not valid.
     * (Required)
     * 
     * @return
     *     The field
     */
    @JsonProperty("field")
    public String getField() {
        return field;
    }

    /**
     * The field whose value is not valid.
     * (Required)
     * 
     * @param field
     *     The field
     */
    @JsonProperty("field")
    public void setField(String field) {
        this.field = field;
    }

    /**
     * The reason message is to be used by a developer trying to understand what a code means.
     * 
     * The message is not expected to be rendered to the end user.
     * (Required)
     * 
     * @return
     *     The message
     */
    @JsonProperty("message")
    public String getMessage() {
        return message;
    }

    /**
     * The reason message is to be used by a developer trying to understand what a code means.
     * 
     * The message is not expected to be rendered to the end user.
     * (Required)
     * 
     * @param message
     *     The message
     */
    @JsonProperty("message")
    public void setMessage(String message) {
        this.message = message;
    }

    /**
     * ReasonCode
     * <p>
     * The reasonCode is expected to be used by the client to go to a content management system to determine some content that is to be displayed to the end user.
     * FIELD_REQUIRED:A field that is required was not provided.
     * INVALID_FORMAT:A field was provided but is in the wrong format.
     * 
     * @return
     *     The code
     */
    @JsonProperty("code")
    public ReasonCode getCode() {
        return code;
    }

    /**
     * ReasonCode
     * <p>
     * The reasonCode is expected to be used by the client to go to a content management system to determine some content that is to be displayed to the end user.
     * FIELD_REQUIRED:A field that is required was not provided.
     * INVALID_FORMAT:A field was provided but is in the wrong format.
     * 
     * @param code
     *     The code
     */
    @JsonProperty("code")
    public void setCode(ReasonCode code) {
        this.code = code;
    }

    @Override
    public String toString() {
        return ToStringBuilder.reflectionToString(this);
    }

    @Override
    public int hashCode() {
        return new HashCodeBuilder().append(field).append(message).append(code).toHashCode();
    }

    @Override
    public boolean equals(Object other) {
        if (other == this) {
            return true;
        }
        if ((other instanceof Reasons) == false) {
            return false;
        }
        Reasons rhs = ((Reasons) other);
        return new EqualsBuilder().append(field, rhs.field).append(message, rhs.message).append(code, rhs.code).isEquals();
    }

}
