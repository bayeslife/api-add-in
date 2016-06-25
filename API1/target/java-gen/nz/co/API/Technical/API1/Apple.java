
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
 * Apple
 * <p>
 * 
 * 
 */
@JsonInclude(JsonInclude.Include.NON_NULL)
@Generated("org.jsonschema2pojo")
@JsonPropertyOrder({
    "taste",
    "species"
})
public class Apple {

    /**
     * The taste of the apple
     * (Required)
     * 
     */
    @JsonProperty("taste")
    @NotNull
    private String taste;
    /**
     * The type of apple
     * (Required)
     * 
     */
    @JsonProperty("species")
    @NotNull
    private String species;

    /**
     * The taste of the apple
     * (Required)
     * 
     * @return
     *     The taste
     */
    @JsonProperty("taste")
    public String getTaste() {
        return taste;
    }

    /**
     * The taste of the apple
     * (Required)
     * 
     * @param taste
     *     The taste
     */
    @JsonProperty("taste")
    public void setTaste(String taste) {
        this.taste = taste;
    }

    /**
     * The type of apple
     * (Required)
     * 
     * @return
     *     The species
     */
    @JsonProperty("species")
    public String getSpecies() {
        return species;
    }

    /**
     * The type of apple
     * (Required)
     * 
     * @param species
     *     The species
     */
    @JsonProperty("species")
    public void setSpecies(String species) {
        this.species = species;
    }

    @Override
    public String toString() {
        return ToStringBuilder.reflectionToString(this);
    }

    @Override
    public int hashCode() {
        return new HashCodeBuilder().append(taste).append(species).toHashCode();
    }

    @Override
    public boolean equals(Object other) {
        if (other == this) {
            return true;
        }
        if ((other instanceof Apple) == false) {
            return false;
        }
        Apple rhs = ((Apple) other);
        return new EqualsBuilder().append(taste, rhs.taste).append(species, rhs.species).isEquals();
    }

}
