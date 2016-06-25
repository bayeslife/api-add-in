
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
 * Entity
 * <p>
 * 
 * 
 */
@JsonInclude(JsonInclude.Include.NON_NULL)
@Generated("org.jsonschema2pojo")
@JsonPropertyOrder({
    "apple",
    "oranges"
})
public class Entity {

    /**
     * Apple
     * <p>
     * 
     * 
     */
    @JsonProperty("apple")
    @Valid
    private Apple apple;
    /**
     * 
     * 
     */
    @JsonProperty("oranges")
    @Valid
    private List<Orange> oranges = new ArrayList<Orange>();

    /**
     * Apple
     * <p>
     * 
     * 
     * @return
     *     The apple
     */
    @JsonProperty("apple")
    public Apple getApple() {
        return apple;
    }

    /**
     * Apple
     * <p>
     * 
     * 
     * @param apple
     *     The apple
     */
    @JsonProperty("apple")
    public void setApple(Apple apple) {
        this.apple = apple;
    }

    /**
     * 
     * 
     * @return
     *     The oranges
     */
    @JsonProperty("oranges")
    public List<Orange> getOranges() {
        return oranges;
    }

    /**
     * 
     * 
     * @param oranges
     *     The oranges
     */
    @JsonProperty("oranges")
    public void setOranges(List<Orange> oranges) {
        this.oranges = oranges;
    }

    @Override
    public String toString() {
        return ToStringBuilder.reflectionToString(this);
    }

    @Override
    public int hashCode() {
        return new HashCodeBuilder().append(apple).append(oranges).toHashCode();
    }

    @Override
    public boolean equals(Object other) {
        if (other == this) {
            return true;
        }
        if ((other instanceof Entity) == false) {
            return false;
        }
        Entity rhs = ((Entity) other);
        return new EqualsBuilder().append(apple, rhs.apple).append(oranges, rhs.oranges).isEquals();
    }

}
