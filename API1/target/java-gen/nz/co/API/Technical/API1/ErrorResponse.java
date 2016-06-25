
package nz.co.API.Technical.API1;

import javax.annotation.Generated;
import javax.validation.Valid;
import com.fasterxml.jackson.annotation.JsonInclude;
import com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.annotation.JsonPropertyOrder;
import org.apache.commons.lang3.builder.EqualsBuilder;
import org.apache.commons.lang3.builder.HashCodeBuilder;
import org.apache.commons.lang3.builder.ToStringBuilder;


/**
 * ErrorResponse
 * <p>
 * 
 * 
 */
@JsonInclude(JsonInclude.Include.NON_NULL)
@Generated("org.jsonschema2pojo")
@JsonPropertyOrder({
    "responseInfo"
})
public class ErrorResponse {

    /**
     * ResponseInfo
     * <p>
     * Response  Info
     * 
     */
    @JsonProperty("responseInfo")
    @Valid
    private ResponseInfo responseInfo;

    /**
     * ResponseInfo
     * <p>
     * Response  Info
     * 
     * @return
     *     The responseInfo
     */
    @JsonProperty("responseInfo")
    public ResponseInfo getResponseInfo() {
        return responseInfo;
    }

    /**
     * ResponseInfo
     * <p>
     * Response  Info
     * 
     * @param responseInfo
     *     The responseInfo
     */
    @JsonProperty("responseInfo")
    public void setResponseInfo(ResponseInfo responseInfo) {
        this.responseInfo = responseInfo;
    }

    @Override
    public String toString() {
        return ToStringBuilder.reflectionToString(this);
    }

    @Override
    public int hashCode() {
        return new HashCodeBuilder().append(responseInfo).toHashCode();
    }

    @Override
    public boolean equals(Object other) {
        if (other == this) {
            return true;
        }
        if ((other instanceof ErrorResponse) == false) {
            return false;
        }
        ErrorResponse rhs = ((ErrorResponse) other);
        return new EqualsBuilder().append(responseInfo, rhs.responseInfo).isEquals();
    }

}
